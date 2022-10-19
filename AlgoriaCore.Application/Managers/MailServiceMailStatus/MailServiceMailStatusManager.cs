using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailStatuss.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailStatuss
{
    public class MailServiceMailStatusManager : BaseManager
    {
        private readonly IRepository<MailServiceMailStatus, long> _repMailServiceMailStatus;
        private readonly IRepository<MailServiceMail, long> _repMailServiceMail;
        public MailServiceMailStatusManager(IRepository<MailServiceMailStatus, long> repMailServiceMailStatus
                           , IRepository<MailServiceMail, long> repMailServiceMail
        )
        {
            _repMailServiceMailStatus = repMailServiceMailStatus;
            _repMailServiceMail = repMailServiceMail;
        }

        public async Task<MailServiceMailStatusDto> GetMailServiceMailStatusByHostAsync(long id, bool throwExceptionIfNotFound = true)
        {
            using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
            {
                using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                {
                    var query = GetMailServiceMailStatusQuery();
                    var dto = await query.FirstOrDefaultAsync(p => p.MailServiceMail == id);
                    if (throwExceptionIfNotFound && dto == null)
                    {
                        throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("MailServiceMailStatuss.MailServiceMailStatus"), id));
                    }
                    return dto;
                }
            }

        }

        public async Task<MailServiceMailStatusDto> GetMailServiceMailStatusAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetMailServiceMailStatusQuery();
            MailServiceMailStatusDto dto = await query.FirstOrDefaultAsync(p => p.MailServiceMail == id);
            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("MailServiceMailStatuss.MailServiceMailStatus"), id));
            }
            return dto;
        }

        private IQueryable<MailServiceMailStatusDto> GetMailServiceMailStatusQuery()
        {
            string status1 = L("MailServiceMailStatuss.MailServiceMailStatus.Status.Pending");
            string status2 = L("MailServiceMailStatuss.MailServiceMailStatus.Status.Success");
            string status3 = L("MailServiceMailStatuss.MailServiceMailStatus.Status.Error");
            string status4 = L("MailServiceMailStatuss.MailServiceMailStatus.Status.Canceled");

            var query = (from entity in _repMailServiceMailStatus.GetAll()
                         join mailServiceMail in _repMailServiceMail.GetAll() on entity.MailServiceMail equals mailServiceMail.Id into mailServiceMailX
                         from mailServiceMail in mailServiceMailX.DefaultIfEmpty()
                         select new MailServiceMailStatusDto
                         {
                             MailServiceMail = entity.MailServiceMail,
                             SentTime = entity.SentTime,
                             Status = entity.Status,
                             StatusDesc =
                              entity.Status == 0 ? status1 :
                              entity.Status == 2 ? status2 :
                              entity.Status == 3 ? status3 :
                              entity.Status == 4 ? status4 : string.Empty,
                             Error = entity.Error,
                             Id = entity.Id,
                         });
            return query;
        }
    }
}

