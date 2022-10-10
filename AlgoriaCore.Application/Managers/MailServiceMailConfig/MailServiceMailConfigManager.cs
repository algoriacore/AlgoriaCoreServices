using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailConfigs.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailConfigs
{
    public class MailServiceMailConfigManager : BaseManager
    {
        private readonly IRepository<MailServiceMailConfig, long> _repMailServiceMailConfig;
        private readonly IRepository<MailServiceMail, long> _repMailServiceMail;
        public MailServiceMailConfigManager(IRepository<MailServiceMailConfig, long> repMailServiceMailConfig
                           , IRepository<MailServiceMail, long> repMailServiceMail
        )
        {
            _repMailServiceMailConfig = repMailServiceMailConfig;
            _repMailServiceMail = repMailServiceMail;
        }

        public async Task<MailServiceMailConfigDto> GetMailServiceMailConfigAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetMailServiceMailConfigQuery();
            MailServiceMailConfigDto dto = await query.FirstOrDefaultAsync(p => p.MailServiceMail == id);
            return dto;
        }


        private IQueryable<MailServiceMailConfigDto> GetMailServiceMailConfigQuery()
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");



            var query = (from entity in _repMailServiceMailConfig.GetAll()
                         join mailServiceMail in _repMailServiceMail.GetAll() on entity.MailServiceMail equals mailServiceMail.Id into mailServiceMailX
                         from mailServiceMail in mailServiceMailX.DefaultIfEmpty()
                         select new MailServiceMailConfigDto
                         {
                             MailServiceMail = entity.MailServiceMail,
                             Sender = entity.Sender,
                             SenderDisplay = entity.SenderDisplay,
                             Smpthost = entity.SMPTHost,
                             Smptport = entity.SMPTPort,
                             IsSsl = entity.IsSSL,
                             IsSslDesc = entity.IsSSL == true ? yesLabel : noLabel,
                             UseDefaultCredential = entity.UseDefaultCredential,
                             UseDefaultCredentialDesc = entity.UseDefaultCredential == true ? yesLabel : noLabel,
                             Domain = entity.Domain,
                             MailUser = entity.MailUser,
                             MailPassword = entity.MailPassword,
                             Id = entity.Id,
                         });
            return query;
        }
    }
}

