using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Groups;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._1Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._2Queries
{
    public class MailGroupGetForEditQueryHandler : BaseCoreClass, IRequestHandler<MailGroupGetForEditQuery, MailGroupForEditResponse>
    {
        private readonly MailGroupManager _manager;

        public MailGroupGetForEditQueryHandler(ICoreServices coreServices, MailGroupManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<MailGroupForEditResponse> Handle(MailGroupGetForEditQuery request, CancellationToken cancellationToken)
        {
            var dto = await _manager.GetMailGroupAsync(request.Id);

            var resp = new MailGroupForEditResponse {
                Id = dto.Id.Value,
                DisplayName = dto.DisplayName,
                Header = dto.Header,
                Footer = dto.Footer
            };

            return resp;
        }
    }
}
