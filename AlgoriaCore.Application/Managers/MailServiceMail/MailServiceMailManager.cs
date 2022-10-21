using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails
{
    public class MailServiceMailManager : BaseManager
    {
        private readonly IRepository<MailServiceMail, long> _repMailServiceMail;
        private readonly IRepository<MailServiceRequest, long> _repMailServiceRequest;
        private readonly IRepository<MailServiceMailStatus, long> _repMailServiceMailStatus;

        public MailServiceMailManager(IRepository<MailServiceMail, long> repMailServiceMail
                           , IRepository<MailServiceRequest, long> repMailServiceRequest,
                             IRepository<MailServiceMailStatus, long> repMailServiceMailStatus
        )
        {
            _repMailServiceMail = repMailServiceMail;
            _repMailServiceRequest = repMailServiceRequest;
            _repMailServiceMailStatus = repMailServiceMailStatus;
        }


        public async Task<PagedResultDto<MailServiceMailDto>> GetMailServiceMailPagedListByHostAsync(MailServiceMailListFilterDto dto)
        {
            List<MailServiceMailDto> lst;
            int tot = 0;

            using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
            {
                using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                {
                    string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();
                    var a = GetMailServiceMailQuery()
                    .WhereIf(dto.StartDate != null && dto.EndDate != null, p => p.MailServiceRequestDate >= dto.StartDate && p.MailServiceRequestDate <= dto.EndDate)
                    .WhereIf(dto.OnlyHost.HasValue && dto.OnlyHost.Value, w => w.TenantId == null)
                    .WhereIf(dto.TenantId.HasValue, w => w.TenantId == dto.TenantId)
                    .WhereIf(!dto.Filter.IsNullOrEmpty(),
                         f =>
                         f.IsLocalConfigDesc.ToUpper().Contains(filter) ||
                         f.Sendto.ToUpper().Contains(filter) ||
                         f.CopyTo.ToUpper().Contains(filter) ||
                         f.Subject.ToUpper().Contains(filter)
                    );

                    tot = await a.CountAsync();
                    lst = await a.OrderBy(dto.Sorting).PageBy(dto).ToListAsync();
                }
            }


            return new PagedResultDto<MailServiceMailDto>(tot, lst);
        }

        public async Task<MailServiceMailDto> GetMailServiceMailByHostAsync(long id, bool throwExceptionIfNotFound = true)
        {
            using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
            {
                using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                {
                    var query = GetMailServiceMailQuery();
                    var dto = await query.FirstOrDefaultAsync(p => p.Id == id);
                    if (throwExceptionIfNotFound && dto == null)
                    {
                        throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("MailServiceMails.MailServiceMail"), id));
                    }
                    return dto;
                }
            }

        }

        public async Task<PagedResultDto<MailServiceMailDto>> GetMailServiceMailPagedListAsync(MailServiceMailListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();
            var a = GetMailServiceMailQuery()
             .WhereIf(dto.StartDate != null && dto.EndDate != null, p => p.MailServiceRequestDate >= dto.StartDate && p.MailServiceRequestDate <= dto.EndDate)
            .WhereIf(!dto.Filter.IsNullOrEmpty(),
                 f =>
                 f.IsLocalConfigDesc.ToUpper().Contains(filter) ||
                 f.Sendto.ToUpper().Contains(filter) ||
                 f.CopyTo.ToUpper().Contains(filter) ||
                 f.Subject.ToUpper().Contains(filter)
            );
            var tot = await a.CountAsync();
            var lst = await a.OrderBy(dto.Sorting).PageBy(dto).ToListAsync();
            return new PagedResultDto<MailServiceMailDto>(tot, lst);
        }


        public async Task<MailServiceMailDto> GetMailServiceMailAsync(long id, bool throwExceptionIfNotFound = true)
        {

            var query = GetMailServiceMailQuery();
            var dto = await query.FirstOrDefaultAsync(p => p.Id == id);
            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("MailServiceMails.MailServiceMail"), id));
            }
            return dto;
        }


        private IQueryable<MailServiceMailDto> GetMailServiceMailQuery()
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");

            string status1 = L("MailServiceMailStatuss.MailServiceMailStatus.Status.Pending");
            string status2 = L("MailServiceMailStatuss.MailServiceMailStatus.Status.Success");
            string status3 = L("MailServiceMailStatuss.MailServiceMailStatus.Status.Error");
            string status4 = L("MailServiceMailStatuss.MailServiceMailStatus.Status.Canceled");

			var query = (from entity in _repMailServiceMail.GetAll()
                         join mailServiceRequest in _repMailServiceRequest.GetAll() on entity.MailServiceRequest equals mailServiceRequest.Id into mailServiceRequestX
                         from mailServiceRequest in mailServiceRequestX.DefaultIfEmpty()
                         join mailServiceMailStatus in _repMailServiceMailStatus.GetAll() on entity.Id equals mailServiceMailStatus.MailServiceMail into mailServiceMailStatusX
                         from mailServiceMailStatus in mailServiceMailStatusX.DefaultIfEmpty()
                         select new MailServiceMailDto
                         {
                             Id = entity.Id,
                             TenantId = entity.TenantId,
                             MailServiceRequest = entity.MailServiceRequest,
                             MailServiceRequestDate = mailServiceRequest.CreationTime,
                             IsLocalConfig = entity.IsLocalConfig,
                             IsLocalConfigDesc = entity.IsLocalConfig ? yesLabel : noLabel,
                             Sendto = entity.Sendto,
                             CopyTo = entity.CopyTo,
                             BlindCopyTo = entity.BlindCopyTo,
                             Subject = entity.Subject,
                             Status = mailServiceMailStatus.Status,
                             StatusDesc = mailServiceMailStatus.Status == 1 ? status1 :
                                        (mailServiceMailStatus.Status == 2 ? status2 :
                                        (mailServiceMailStatus.Status == 3 ? status3 :
                                        (mailServiceMailStatus.Status == 4 ? status4 : string.Empty)))
                         });

            return query;
        }
    }
}

