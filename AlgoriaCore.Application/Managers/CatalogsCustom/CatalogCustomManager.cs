using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.CatalogsCustom.Dto;
using AlgoriaCore.Application.Managers.Questionnaires;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Entities.MongoDb;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.CatalogsCustom
{
    public class CatalogCustomManager : BaseManager
    {
        private readonly IRepositoryMongoDb<CatalogCustom> _repository;
        private readonly IRepositoryMongoDb<Questionnaire> _repositoryQuestionnaire;
        private readonly IRepository<User, long> _repositoryUser;

        private readonly IMongoDBContext _context;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;

        public CatalogCustomManager(
            IRepositoryMongoDb<CatalogCustom> repository,
            IRepositoryMongoDb<Questionnaire> repositoryQuestionnaire,
            IRepository<User, long> repositoryUser,
            IMongoDBContext context,
            IMongoUnitOfWork mongoUnitOfWork
        )
        {
            _repository = repository;
            _repositoryQuestionnaire = repositoryQuestionnaire;
            _repositoryUser = repositoryUser;

            _context = context;
            _mongoUnitOfWork = mongoUnitOfWork;
        }

        #region CatalogCustom

        public async Task<PagedResultDto<CatalogCustomDto>> GetCatalogCustomListAsync(CatalogCustomListFilterDto dto)
        {
            var query = GetCatalogCustomListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "Description" : dto.Sorting)
                .PageBy(dto)
                .ToListAsync();

            return new PagedResultDto<CatalogCustomDto>(count, ll);
        }

        public List<CatalogCustomDto> GetCatalogCustomActiveListAsync()
        {
            var query = GetCatalogCustomQuery();

            return query.Where(c => c.IsActive).ToList();
        }

        public async Task<List<ComboboxItemDto>> GetCatalogCustomComboAsync()
        {
            return await GetCatalogCustomComboAsync(new CatalogCustomComboFilterDto() { IsActive = true });
        }

        public async Task<List<ComboboxItemDto>> GetCatalogCustomComboAsync(CatalogCustomComboFilterDto dto)
        {
            var query = _repository.GetAll()
                .WhereIf(dto.IsActive != null, p => p.IsActive == dto.IsActive)
                .WhereIf(!dto.Filter.IsNullOrWhiteSpace(), p => p.Description.Contains(dto.Filter))
                .OrderBy(p => p.Description)
                .Select(p => new ComboboxItemDto
                {
                    Value = p.Id,
                    Label = p.Description
                });

            return await query.ToListAsync();
        }

        public async Task<CatalogCustomDto> GetCatalogCustomAsync(string id, bool throwExceptionIfNotFound = true)
        {
            var query = GetCatalogCustomQuery();

            CatalogCustomDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("CatalogCustoms.CatalogCustom"), id));
            }

            return dto;
        }

        public async Task<List<CatalogCustomDto>> GetCatalogCustomByNameSingularListAsync(string nameSingular)
        {
            var query = GetCatalogCustomQuery();

            return await query.Where(p => p.NameSingular.ToUpper() == nameSingular.ToUpper()).ToListAsync();
        }

        public async Task<string> CreateCatalogCustomAsync(CatalogCustomDto dto)
        {
            await ValidateCatalogCustomAsync(dto);

            var entity = new CatalogCustom();

            entity.NameSingular = dto.NameSingular;
            entity.NamePlural = dto.NamePlural;
            entity.CollectionName = CalculateCollectionName(dto.NameSingular);
            entity.Description = dto.Description;
            entity.IsCollectionGenerated = false;
            entity.CreationDateTime = DateTime.UtcNow;
            entity.UserCreator = _repositoryUser.GetAll().Where(p => p.Id == SessionContext.UserId)
                .Select(p => (p.Name + " " + p.Lastname + " " + p.SecondLastname).Trim()).First();
            entity.IsActive = dto.IsActive;
            entity.Questionnarie = dto.Questionnaire;
            entity.FieldNames = dto.FieldNames;

            entity.Id = await _repository.InsertAndGetIdAsync(entity);

            await GenerateDbCollectionAsync(entity.Id);

            entity.IsCollectionGenerated = true;
            await _repository.UpdateAsync(entity);

            return entity.Id;
        }

        public async Task UpdateCatalogCustomAsync(CatalogCustomDto dto)
        {
            CatalogCustomDto previousDto = await GetCatalogCustomAsync(dto.Id);

            dto.CollectionName = previousDto.IsCollectionGenerated ? previousDto.CollectionName : CalculateCollectionName(dto.NameSingular);

            await ValidateCatalogCustomAsync(dto);

            var entity = await _repository.FirstOrDefaultAsync(dto.Id);

            entity.NameSingular = dto.NameSingular;
            entity.NamePlural = dto.NamePlural;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.Questionnarie = dto.Questionnaire;
            entity.FieldNames = dto.FieldNames;

            if (entity.IsCollectionGenerated != true)
            {
                entity.CollectionName = dto.CollectionName;
            }

            await GenerateDbCollectionAsync(entity.Id);

            entity.IsCollectionGenerated = true;

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteCatalogCustomAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task GenerateDbCollectionAsync(string catalog)
        {
            CatalogCustomDto dto = await GetCatalogCustomAsync(catalog);

            bool exists = await (await _context.Database.ListCollectionNamesAsync(new ListCollectionNamesOptions
            {
                Filter = new BsonDocument("name", dto.CollectionName)
            })).AnyAsync();

            if (!exists) {
                await _context.Database.CreateCollectionAsync(_mongoUnitOfWork.GetSession(), dto.CollectionName, new CreateCollectionOptions
                {
                    Collation = new Collation(locale: "es", strength: CollationStrength.Primary, numericOrdering: true),
                });
            }
        }

        #region Private Methods

        private IMongoQueryable<CatalogCustomDto> GetCatalogCustomQuery()
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");

            var query = from entity in _repository.GetAll()
                        join Q in _repositoryQuestionnaire.GetCollection() on entity.Questionnarie equals Q.Id
                        select new CatalogCustomDto
                        {
                            Id = entity.Id,
                            CreationDateTime = entity.CreationDateTime,
                            CollectionName = entity.CollectionName,
                            IsCollectionGenerated = entity.IsCollectionGenerated,
                            IsCollectionGeneratedDesc = entity.IsCollectionGenerated == true ? yesLabel : noLabel,
                            Description = entity.Description,
                            NamePlural = entity.NamePlural,
                            NameSingular = entity.NameSingular,
                            Questionnaire = entity.Questionnarie,
                            QuestionnaireDesc = Q.Name,
                            UserCreator = entity.UserCreator,
                            IsActive = entity.IsActive,
                            IsActiveDesc = entity.IsActive == true ? yesLabel : noLabel,
                            FieldNames = entity.FieldNames
                        };

            return query;
        }

        private IMongoQueryable<CatalogCustomDto> GetCatalogCustomListQuery(CatalogCustomListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetCatalogCustomQuery()
                .WhereIf(
                    filter != null,
                    p => p.Id.Contains(filter)
                    || p.CollectionName.ToUpper().Contains(filter)
                    || p.IsCollectionGeneratedDesc.ToUpper().Contains(filter)
                    || p.Description.ToUpper().Contains(filter)
                    || p.NamePlural.ToUpper().Contains(filter)
                    || p.NameSingular.ToUpper().Contains(filter)
                    || p.UserCreator.ToUpper().Contains(filter)
                    || p.IsActiveDesc.ToUpper().Contains(filter)
                );

            return query;
        }

        private async Task ValidateCatalogCustomAsync(CatalogCustomDto dto)
        {
            List<CatalogCustomDto> list = await GetCatalogCustomByNameSingularListAsync(dto.NameSingular);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("CatalogsCustom.CatalogCustom.DuplicatedName"));
            }
        }

        private string CalculateCollectionName(string name)
        {
            int? tenantId = CurrentUnitOfWork.GetTenantId();

            return "UDT_" + (tenantId.HasValue ? tenantId.ToString() + "_" : "") + name.ToVariableName();
        }

        #endregion

        #endregion

        #region CatalogCustom Field

        public async Task<List<ComboboxItemDto>> GetCatalogCustomFieldComboAsync(CatalogCustomFieldComboFilterDto dto)
        {
            var questionnaire = await _repository.GetAll()
                            .Where(p => p.Id == dto.CatalogCustom)
                            .Select(p => p.Questionnarie)
                            .FirstOrDefaultAsync();

            var query = from Q in _repositoryQuestionnaire.GetAll()
                        .WhereIf(questionnaire != null, p => p.Id == questionnaire)
                        select Q.Sections into SS
                        from S in SS select S.Fields into FF
                        from F in FF
                        orderby F.Name
                        select new ComboboxItemDto
                        {
                            Value = F.FieldName,
                            Label = F.Name
                        };

            return await query.ToListAsync();
        }

        #endregion
    }
}
