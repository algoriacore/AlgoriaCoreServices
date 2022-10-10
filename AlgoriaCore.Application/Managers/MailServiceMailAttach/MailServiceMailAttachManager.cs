using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailAttachs.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailAttachs
{
    public class MailServiceMailAttachManager : BaseManager
  {
      private readonly IRepository<MailServiceMailAttach, long> _repMailServiceMailAttach;
      private readonly IRepository<MailServiceMailBody, long> _repMailServiceMailBody;
      public MailServiceMailAttachManager (IRepository<MailServiceMailAttach, long> repMailServiceMailAttach
                         , IRepository<MailServiceMailBody, long> repMailServiceMailBody
      )
      {
          _repMailServiceMailAttach = repMailServiceMailAttach;
          _repMailServiceMailBody = repMailServiceMailBody;
      }

      public async Task<PagedResultDto<MailServiceMailAttachDto>> GetMailServiceMailAttachPagedListAsync(MailServiceMailAttachListFilterDto dto)
      {
             string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();
             var a = GetMailServiceMailAttachQuery()
             .Where(w =>  w.MailServiceMailBody == dto.MailServiceMailBody)
             .WhereIf(!dto.Filter.IsNullOrEmpty(), 
                  f => 
                  f.ContenType.ToUpper().Contains(filter) ||
                  f.FileName.ToUpper().Contains(filter)
             );
             var tot = await a.CountAsync();
             var lst = await a.OrderBy(dto.Sorting).PageBy(dto).ToListAsync();
             return new PagedResultDto<MailServiceMailAttachDto>(tot, lst);
      }
      public async Task<MailServiceMailAttachDto> GetMailServiceMailAttachAsync(long id, bool throwExceptionIfNotFound = true)
      {
            var query = GetMailServiceMailAttachQuery();
            MailServiceMailAttachDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);
             if (throwExceptionIfNotFound && dto == null)
             {
             throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("MailServiceMailAttachs.MailServiceMailAttach"), id));
             }
             return dto;
      }

      private IQueryable<MailServiceMailAttachDto> GetMailServiceMailAttachQuery() 
      {
             string yesLabel = L("Yes");
             string noLabel = L("No");

             var query = (from entity in _repMailServiceMailAttach.GetAll()
             join mailServiceMailBody in _repMailServiceMailBody.GetAll() on entity.MailServiceMailBody equals mailServiceMailBody.Id into mailServiceMailBodyX
             from mailServiceMailBody in mailServiceMailBodyX.DefaultIfEmpty()
             select new MailServiceMailAttachDto
             {
                  MailServiceMailBody = entity.MailServiceMailBody,
                  ContenType = entity.ContenType,
                  FileName = entity.FileName,
                  BinaryFile = entity.BinaryFile,
                  Id = entity.Id,
             });
             return query;
      }
  }
}

