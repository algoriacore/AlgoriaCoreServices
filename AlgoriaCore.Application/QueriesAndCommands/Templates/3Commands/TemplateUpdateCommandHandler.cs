using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.BinaryObjects;
using AlgoriaCore.Application.Managers.BinaryObjects.Dto;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using AlgoriaCore.Extensions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateUpdateCommandHandler : BaseCoreClass, IRequestHandler<TemplateUpdateCommand, long>
    {
        private readonly TemplateManager _managerTemplate;
        private readonly BinaryObjectManager _managerBinaryObject;

        public TemplateUpdateCommandHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        , BinaryObjectManager managerBinaryObject
        ) : base(coreServices)
        {
            _managerTemplate = managerTemplate;
            _managerBinaryObject = managerBinaryObject;
        }

        public async Task<long> Handle(TemplateUpdateCommand request, CancellationToken cancellationToken)
        {
            TemplateDto dto = new TemplateDto()
            {
                Id = request.Id,
                RGBColor = request.RGBColor,
                NameSingular = request.NameSingular,
                NamePlural = request.NamePlural,
                Description = request.Description,
                HasChatRoom = request.HasChatRoom,
                IsActivity = request.IsActivity,
                HasSecurity = request.HasSecurity,
                IsActive = request.IsActive
            };

            if (!request.IconName.IsNullOrWhiteSpace())
            {
                FileDto iconFile = await _managerBinaryObject.GetTempFileAsync(request.IconName);

                if (iconFile != null)
                {
                    dto.IconBytes = iconFile.FileArray;
                }
            }

            await _managerTemplate.UpdateTemplateAsync(dto);

            return dto.Id.Value;
        }
    }
}
