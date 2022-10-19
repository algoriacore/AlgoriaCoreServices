using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailBodys.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailBodys
{
    public class MailServiceMailBodyManager : BaseManager
    {
        private readonly IRepository<MailServiceMailBody, long> _repMailServiceMailBody;
        private readonly IRepository<MailServiceMail, long> _repMailServiceMail;
        public MailServiceMailBodyManager(IRepository<MailServiceMailBody, long> repMailServiceMailBody
                           , IRepository<MailServiceMail, long> repMailServiceMail
        )
        {
            _repMailServiceMailBody = repMailServiceMailBody;
            _repMailServiceMail = repMailServiceMail;
        }


        public async Task<MailServiceMailBodyDto> GetMailServiceMailBodyByHostAsync(long id, bool throwExceptionIfNotFound = true)
        {
            using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
            {
                using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                {
                    var query = GetMailServiceMailBodyQuery();
                    var dto = await query.FirstOrDefaultAsync(p => p.MailServiceMail == id);
                    if (throwExceptionIfNotFound && dto == null)
                    {
                        throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("MailServiceMailBodys.MailServiceMailBody"), id));
                    }
                    return dto;
                }
            }

        }

        public async Task<MailServiceMailBodyDto> GetMailServiceMailBodyAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetMailServiceMailBodyQuery();
            MailServiceMailBodyDto dto = await query.FirstOrDefaultAsync(p => p.MailServiceMail == id);
            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("MailServiceMailBodys.MailServiceMailBody"), id));
            }
            return dto;
        }

        private IQueryable<MailServiceMailBodyDto> GetMailServiceMailBodyQuery()
        {
            var query = (from entity in _repMailServiceMailBody.GetAll()
                         join mailServiceMail in _repMailServiceMail.GetAll() on entity.MailServiceMail equals mailServiceMail.Id into mailServiceMailX
                         from mailServiceMail in mailServiceMailX.DefaultIfEmpty()
                         select new MailServiceMailBodyDto
                         {
                             MailServiceMail = entity.MailServiceMail,
                             Body = entity.Body,
                             Id = entity.Id,
                         });
            return query;
        }
    }
}

