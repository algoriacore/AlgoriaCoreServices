using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.CatalogsCustom.Dto;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl.Dto;
using AlgoriaCore.Application.Managers.Questionnaires;
using AlgoriaCore.Application.Managers.Questionnaires.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Entities.MongoDb;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using lizzie;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.CatalogsCustomImpl
{
    public class CatalogCustomImplManager : BaseManager
    {
        private readonly CatalogCustomManager _managerCatalogCustom;
        private readonly QuestionnaireManager _managerQuestionnaire;

        private readonly IMongoDBContext _context;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;

        private CatalogCustomDto _catalogCustomDto;
        private QuestionnaireDto _questionnaireDto;

        private IMongoCollection<BsonDocument> _collection;

        private string PrefixObj = "_obj";

        public CatalogCustomImplManager(
            CatalogCustomManager managerCatalogCustom,
            QuestionnaireManager managerQuestionnaire,
            IMongoDBContext context,
            IMongoUnitOfWork mongoUnitOfWork
        )
        {
            _managerQuestionnaire = managerQuestionnaire;
            _managerCatalogCustom = managerCatalogCustom;

            _context = context;
            _mongoUnitOfWork = mongoUnitOfWork;
        }

        public async Task SetCatalogCustom(string catalog)
        {
            _catalogCustomDto = await _managerCatalogCustom.GetCatalogCustomAsync(catalog);
            _questionnaireDto = await _managerQuestionnaire.GetQuestionnaireAsync(_catalogCustomDto.Questionnaire);
            _collection = _context.Database.GetCollection<BsonDocument>(_catalogCustomDto.CollectionName);
        }

        //public async Task<PagedResultDto<BsonDocument>> GetCatalogCustomImplListAsync(CatalogCustomImplListFilterDto dto)
        //{
        //    var query = GetCatalogCustomImplListQuery(dto);
        //    var countResult = await query.Count().FirstOrDefaultAsync();
        //    var count = countResult == null ? 0 : (int)countResult.Count;

        //    var ll = await query
        //        .OrderBy(dto.Sorting.IsNullOrEmpty() ? "_id" : dto.Sorting)
        //        .PageBy(dto)
        //        .ToListAsync();

        //    return new PagedResultDto<BsonDocument>(count, ll);
        //}

        //public async Task<PagedResultDto<BsonDocument>> GetCatalogCustomImplListAsync(CatalogCustomImplListFilterDto dto)
        //{
        //    BsonDocument filter = BsonDocument.Parse("{ \"$or\" : [{ \"nombre_completo\" : /FER/i }, { \"edad\" : /FER/i }, { \"sexo\" : /FER/i }, { \"estado_civil\" : /FER/i }, { \"fecha_de_nacimiento\" : /FER/i }, { \"rfc\" : /FER/i }, { \"curp\" : /FER/i }, { \"nss\" : /FER/i }] }");
        //    BsonDocument sorting = BsonDocument.Parse("{ \"_id\": 1 }");
        //    BsonDocument projection = BsonDocument.Parse("{ \"_id\" : 1, \"nombre_completo\" : 1, \"edad\" : 1, \"sexo\" : 1, \"estado_civil\" : 1, \"fecha_de_nacimiento\" : 1, \"rfc\" : 1, \"curp\" : 1, \"nss\" : 1 } ");

        //    var ll = await _collection
        //        .Find(filter)
        //        .OrderBy(dto.Sorting.IsNullOrEmpty() ? "_id" : dto.Sorting)
        //        .Project(projection)
        //        .PageBy(dto)
        //        .ToListAsync();

        //    return new PagedResultDto<BsonDocument>(100002, ll);
        //}

        public async Task<PagedResultDto<BsonDocument>> GetCatalogCustomImplListAsync(CatalogCustomImplListFilterDto dto)
        {
            var query = GetCatalogCustomImplListQuery(dto);
            var count = await query.CountDocumentsAsync();
            // long count = 100002;

            var ll = await query
                .PageBy(dto)
                .ToListAsync();

            return new PagedResultDto<BsonDocument>((int)count, ll);
        }

        //        public async Task<PagedResultDto<BsonDocument>> GetCatalogCustomImplListAsync(CatalogCustomImplListFilterDto dto)
        //        {
        //            BsonDocument filter = BsonDocument.Parse("{ \"$or\" : [{ \"nombre_completo\" : /FER/i }, { \"edad\" : /FER/i }, { \"sexo\" : /FER/i }, { \"estado_civil\" : /FER/i }, { \"fecha_de_nacimiento\" : /FER/i }, { \"rfc\" : /FER/i }, { \"curp\" : /FER/i }, { \"nss\" : /FER/i }] }");
        //            BsonDocument sorting = BsonDocument.Parse("{ \"_id\": 1 }");
        //            BsonDocument projection = BsonDocument.Parse("{ \"_id\" : 1, \"nombre_completo\" : 1, \"edad\" : 1, \"sexo\" : 1, \"estado_civil\" : 1, \"fecha_de_nacimiento\" : 1, \"rfc\" : 1, \"curp\" : 1, \"nss\" : 1 } ");

        //            var countFacet = AggregateFacet.Create("count",
        //    PipelineDefinition<BsonDocument, AggregateCountResult>.Create(new[]
        //    {
        //        PipelineStageDefinitionBuilder.Count<BsonDocument>()
        //    }));

        //            var dataFacet = AggregateFacet.Create("data",
        //            PipelineDefinition<BsonDocument, BsonDocument>.Create(new[]
        //            {
        //        PipelineStageDefinitionBuilder.Skip<BsonDocument>(0),
        //        PipelineStageDefinitionBuilder.Limit<BsonDocument>(10),
        //            }));

        //            var aggregation = await _collection.Aggregate()
        //                .Match(filter)
        //                .Facet(countFacet, dataFacet)
        //                .ToListAsync();

        //            var count = aggregation.First()
        //.Facets.First(x => x.Name == "count")
        //.Output<AggregateCountResult>()
        //.First()
        //.Count;

        //            var ll = aggregation.First()
        //                .Facets.First(x => x.Name == "data")
        //                .Output<BsonDocument>()
        //                .ToList();

        //            return new PagedResultDto<BsonDocument>((int)count, ll);
        //        }


        //public async Task<PagedResultDto<BsonDocument>> GetCatalogCustomImplListAsync(CatalogCustomImplListFilterDto dto)
        //{
        //    BsonDocument filter = BsonDocument.Parse("{ \"$or\" : [{ \"nombre_completo\" : /FER/i }, { \"edad\" : /FER/i }, { \"sexo\" : /FER/i }, { \"estado_civil\" : /FER/i }, { \"fecha_de_nacimiento\" : /FER/i }, { \"rfc\" : /FER/i }, { \"curp\" : /FER/i }, { \"nss\" : /FER/i }] }");
        //    BsonDocument grouping = BsonDocument.Parse("{ _id: \"$command\", uses: { $sum: 1 } }");
        //    BsonDocument sorting = BsonDocument.Parse("{ \"uses\": -1 }");

        //    var doc = await _collection.Aggregate()
        //                .Match(filter)
        //                .Group(grouping)
        //                .Sort(sorting)
        //                .FirstOrDefaultAsync();

        //    return new PagedResultDto<BsonDocument>(0, new List<BsonDocument>());
        //}


        //public async Task<PagedResultDto<BsonDocument>> GetCatalogCustomImplListAsync(CatalogCustomImplListFilterDto dto)
        //{
        //    BsonDocument filter = BsonDocument.Parse("{ \"$or\" : [{ \"nombre_completo\" : /FER/i }, { \"edad\" : /FER/i }, { \"sexo\" : /FER/i }, { \"estado_civil\" : /FER/i }, { \"fecha_de_nacimiento\" : /FER/i }, { \"rfc\" : /FER/i }, { \"curp\" : /FER/i }, { \"nss\" : /FER/i }] }");
        //    BsonDocument projection = BsonDocument.Parse("{ \"uses\": 1 }");

        //    var doc = await _collection.AsQueryable().Where(p => p.GetValue("nombre_completo").AsString.ToUpper().Contains("FER")).CountAsync();

        //    return new PagedResultDto<BsonDocument>(0, new List<BsonDocument>());
        //}

        public async Task<List<ComboboxItemDto>> GetCatalogCustomImplComboAsync(CatalogCustomImplComboFilterDto dto)
        {
            List<ComboboxItemDto> combo = new List<ComboboxItemDto>();
            CatalogCustomDto catalogCustomDto = await _managerCatalogCustom.GetCatalogCustomAsync(dto.Catalog);
            QuestionnaireDto questionnaireDto = await _managerQuestionnaire.GetQuestionnaireAsync(catalogCustomDto.Questionnaire);
            QuestionnaireFieldDto questionnaireFieldDto = questionnaireDto.Sections.SelectMany(p => p.Fields).First(p => p.FieldName == dto.FieldName);
            IMongoCollection<BsonDocument> collection = _context.Database.GetCollection<BsonDocument>(catalogCustomDto.CollectionName);

            string addFields = string.Format("{{ $addFields: {{ \"Value\": {{ $toString: \"$_id\" }}, \"Label\": {{ $toString: \"${0}\" }} }} }}", questionnaireFieldDto.FieldName);
            string projects = "{ $project: { \"_id\": 0, \"Value\": 1, \"Label\": 1 } }";
            List<BsonDocument> pipeline = BsonSerializer.Deserialize<BsonArray>(string.Format("[{0}, {1}]", addFields, projects))
           .Select(p => p.AsBsonDocument).ToList();

            var aggregate = collection.Aggregate();

            foreach (var pipe in pipeline)
            {
                aggregate = aggregate.AppendStage<BsonDocument>(pipe);
            }

            if (!dto.Filter.IsNullOrWhiteSpace())
            {
                FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Regex("Label", new BsonRegularExpression(
                            new Regex(dto.Filter, RegexOptions.IgnoreCase)
                        ));

                aggregate = aggregate.Match(filterDefinition);
            }

            aggregate = aggregate.OrderBy("Label");

            return await aggregate.As<ComboboxItemDto>().ToListAsync();
        }

        public async Task<BsonDocument> GetCatalogCustomImplAsync(string id, bool throwExceptionIfNotFound = true)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            BsonDocument dto = await (await _collection.FindAsync(_mongoUnitOfWork.GetSession(), filter)).FirstOrDefaultAsync();

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("CatalogsCustom.CatalogCustom"), id));
            }

            return dto;
        }

        public async Task<string> CreateCatalogCustomImplAsync(BsonDocument dto)
        {
            ValidateCatalogCustomImpl(dto, _questionnaireDto.Sections.SelectMany(p => p.Fields).ToList());

            await _collection.InsertOneAsync(_mongoUnitOfWork.GetSession(), dto);

            return dto["_id"].AsObjectId.ToString();
        }

        public async Task UpdateCatalogCustomImplAsync(BsonDocument dto)
        {
            ValidateCatalogCustomImpl(dto, _questionnaireDto.Sections.SelectMany(p => p.Fields).ToList());

            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", dto["_id"].AsObjectId);

            await _collection.ReplaceOneAsync(_mongoUnitOfWork.GetSession(), filter, dto);
        }

        public BsonDocument ParseToBsonDocument(ExpandoObject data)
        {
            BsonDocument bsonDocument = new BsonDocument(data);

            List<QuestionnaireFieldDto> fields = _questionnaireDto.Sections.SelectMany(p => p.Fields).ToList();
            string scalar;
            BsonElement element;

            foreach (QuestionnaireFieldDto field in fields)
            {
                if (field.FieldType == QuestionnaireFieldType.Boolean)
                {
                    scalar = bsonDocument[field.FieldName].IsBoolean
                        && bsonDocument[field.FieldName].AsBoolean ? L("Yes") : L("No");
                    bsonDocument[field.FieldName + PrefixObj] = bsonDocument[field.FieldName];
                    bsonDocument[field.FieldName] = scalar;
                }
                else if (field.FieldType == QuestionnaireFieldType.Integer || field.FieldType == QuestionnaireFieldType.Decimal
                    || field.FieldType == QuestionnaireFieldType.Currency)
                {
                    bsonDocument[field.FieldName] = bsonDocument[field.FieldName].IsBsonNull ? BsonValue.Create(null)
                        : (field.FieldType == QuestionnaireFieldType.Integer ? bsonDocument[field.FieldName].ToInt64() : bsonDocument[field.FieldName].ToDecimal());
                }
                else if (field.FieldType == QuestionnaireFieldType.Multivalue)
                {
                    if (bsonDocument[field.FieldName].IsBsonArray)
                    {
                        scalar = string.Join(", ", bsonDocument[field.FieldName].AsBsonArray.Select(p => p["description"].AsString));
                        bsonDocument[field.FieldName + PrefixObj] = bsonDocument[field.FieldName];
                        bsonDocument[field.FieldName] = scalar;
                    }
                }
                else if (field.FieldType == QuestionnaireFieldType.CatalogCustom || field.FieldType == QuestionnaireFieldType.User || field.MustHaveOptions)
                {
                    if (bsonDocument[field.FieldName].IsBsonDocument)
                    {
                        scalar = bsonDocument[field.FieldName]["description"].AsString;
                        bsonDocument[field.FieldName + PrefixObj] = bsonDocument[field.FieldName];
                        bsonDocument[field.FieldName] = scalar;
                    }
                    else
                    {
                        if (bsonDocument.TryGetElement(field.FieldName + PrefixObj, out element))
                        {
                            bsonDocument.Set(field.FieldName + PrefixObj, BsonValue.Create(null));
                        }
                        else
                        {
                            bsonDocument.Add(field.FieldName + PrefixObj, BsonValue.Create(null));
                        }

                        bsonDocument[field.FieldName] = string.Empty;
                    }
                }
            }

            return bsonDocument;
        }

        public Dictionary<string, object> ParseToDictionary(BsonDocument bsonDocument)
        {
            BsonDocument bsonDocumentAux = new BsonDocument();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            List<QuestionnaireFieldDto> fields = _questionnaireDto.Sections.SelectMany(p => p.Fields).ToList();
            bool existsBsonValue;
            BsonValue bsonValue;

            foreach (QuestionnaireFieldDto field in fields)
            {
                switch(field.FieldType)
                {
                    case QuestionnaireFieldType.Boolean:
                    case QuestionnaireFieldType.Multivalue:
                    case QuestionnaireFieldType.CatalogCustom:
                    case QuestionnaireFieldType.User:
                        existsBsonValue = bsonDocument.TryGetValue(field.FieldName + PrefixObj, out bsonValue);
                        break;
                    default:
                        if (field.MustHaveOptions)
                        {
                            existsBsonValue = bsonDocument.TryGetValue(field.FieldName + PrefixObj, out bsonValue);
                        }
                        else
                        {
                            existsBsonValue = bsonDocument.TryGetValue(field.FieldName, out bsonValue);
                        }
                        break;
                }

                if (existsBsonValue)
                {
                    bsonDocumentAux.Add(field.FieldName, bsonValue);
                }
            }

            return bsonDocumentAux.ToDictionary();
        }

        public async Task DeleteCatalogCustomImplAsync(string id)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));

            await _collection.DeleteOneAsync(_mongoUnitOfWork.GetSession(), filter);
        }

        #region Private Methods

        private IFindFluent<BsonDocument, BsonDocument> GetCatalogCustomImplListQuery(CatalogCustomImplListFilterDto dto)
        {
            IFindFluent<BsonDocument, BsonDocument> query = _collection.Find(new BsonDocument());

            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();
            List<QuestionnaireFieldDto> fields = _questionnaireDto.Sections.SelectMany(p => p.Fields).ToList();
            QuestionnaireFieldDto field;

            List<string> projectsJSON = new List<string>();
            List<string> filtersJSON = new List<string>();

            projectsJSON.Add("\"_id\": 1");

            if (filter != null)
            {
                filtersJSON.Add(string.Format("{{ \"_id\": /{0}/i }}", filter));
            }

            foreach (string fieldName in _catalogCustomDto.FieldNames)
            {
                field = fields.FirstOrDefault(p => p.FieldName.ToLower() == fieldName.ToLower());

                if (field != null)
                {
                    projectsJSON.Add(string.Format("\"{0}\": 1", field.FieldName));

                    if (filter != null)
                    {
                        //filtersJSON.Add(string.Format("{{ \"{0}\": /{1}/i }}", field.FieldName, filter));
                        //filtersJSON.Add(string.Format("{{ \"{0}\": /^{1}/i }}", field.FieldName, filter));
                        //filtersJSON.Add(string.Format("{{ \"{0}\": /{1}/ }}", field.FieldName, filter));
                        filtersJSON.Add(string.Format("{{ \"{0}\": \"{1}\" }}", field.FieldName, filter));
                    }
                }
            }

            if (filtersJSON.Count > 0)
            {
                query = _collection.Find(BsonDocument.Parse(string.Format("{{ \"$or\" : [{0}] }}", string.Join(", ", filtersJSON))));
                //query = _collection.Find(BsonDocument.Parse(string.Format("{{ \"$or\" : [{0}] }}", string.Join(", ", filtersJSON))));
            }

            query = query.OrderBy(dto.Sorting.IsNullOrEmpty() ? "_id" : dto.Sorting);
            query = query.Project(BsonDocument.Parse("{" + string.Join(", ", projectsJSON) + "}"));

            return query;
        }

        private void ValidateCatalogCustomImpl(BsonDocument dto, List<QuestionnaireFieldDto> fields)
        {
            _managerQuestionnaire.ValidateBsonDocument(dto, fields, _questionnaireDto.CustomCode);
        }

        #endregion
    }

    public class CatalogoCustomImplLizzieContext
    {
        private IAppLocalizationProvider _appLocalizationProvider { get; set; }
        private BsonDocument _document { get; set; }

        public CatalogoCustomImplLizzieContext(IAppLocalizationProvider appLocalizationProvider)
        {
            _appLocalizationProvider = appLocalizationProvider;
        }

        public void SetDocument(BsonDocument document)
        {
            _document = document;
        }

        [Bind(Name = "GetDocument")]
        public object GetDocument(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            return _document;
        }

        [Bind(Name = "GetDocumentAsJson")]
        public object GetDocumentAsJson(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            return _document.ToDictionary().ToJsonString();
        }

        [Bind(Name = "GetDocumentAsDictionary")]
        public object GetDocumentAsDictionary(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            return _document.ToDictionary();
        }

        [Bind(Name = "GetDocumentFieldValue")]
        public object GetDocumentFieldValue(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            object result = null;
            BsonValue bsonValue = _document[args.Get<string>(0)];

            if (bsonValue != null)
            {
                result = BsonTypeMapper.MapToDotNetValue(bsonValue);
            }

            return result;
        }

        [Bind(Name = "GetDocumentFieldValueAsDocument")]
        public object GetDocumentFieldValueAsDocument(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            object result = null;
            BsonValue bsonValue = _document[args.Get<string>(0)];

            if (bsonValue != null)
            {
                result = bsonValue.AsBsonDocument;
            }

            return result;
        }

        [Bind(Name = "GetSubdocumentAsJson")]
        public object GetSubdocumentAsJson(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            BsonDocument document = ((BsonDocument)args.Get(0));

            return document.ToDictionary().ToJsonString();
        }

        [Bind(Name = "GetSubdocumentAsDictionary")]
        public object GetSubdocumentAsDictionary(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            BsonDocument document = ((BsonDocument)args.Get(0));

            return document.ToDictionary();
        }

        [Bind(Name = "GetSubdocumentFieldValue")]
        public object GetSubdocumentFieldValue(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            object result = null;
            BsonDocument document = ((BsonDocument)args.Get(0));
            BsonValue bsonValue = document[args.Get<string>(1)];

            if (bsonValue != null)
            {
                result = BsonTypeMapper.MapToDotNetValue(bsonValue);
            }

            return result;
        }

        [Bind(Name = "GetSubdocumentFieldValueAsDocument")]
        public object GetSubdocumentFieldValueAsDocument(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            object result = null;
            BsonDocument document = ((BsonDocument)args.Get(0));
            BsonValue bsonValue = _document[args.Get<string>(1)];

            if (bsonValue != null)
            {
                result = bsonValue.AsBsonDocument;
            }

            return result;
        }

        [Bind(Name = "L")]
        public object L(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            return _appLocalizationProvider.L(args.Get<string>(0));
        }

        [Bind(Name = "ThrowException")]
        public object ThrowException(Binder<CatalogoCustomImplLizzieContext> context, Arguments args)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var arg in args)
            {
                var resultCustomValidationType = arg.GetType();

                if (resultCustomValidationType.IsGenericType && arg is IEnumerable)
                {
                    List<object> list = (List<object>)arg;

                    if (list.Count > 0)
                    {
                        sb.Append(string.Join(Environment.NewLine, list));
                    }
                }
                else
                {
                    sb.Append(Environment.NewLine + arg);
                }
            }

            throw new AlgoriaCoreGeneralException(sb.ToString());
        }
    }
}
