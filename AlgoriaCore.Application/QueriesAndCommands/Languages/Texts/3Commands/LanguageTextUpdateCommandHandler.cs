using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextUpdateCommandHandler : BaseCoreClass, IRequestHandler<LanguageTextUpdateCommand, long>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageTextUpdateCommandHandler(ICoreServices coreServices
        , LanguageManager managerLanguage): base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<long> Handle(LanguageTextUpdateCommand request, CancellationToken cancellationToken)
        {
            LanguageTextDto dto = new LanguageTextDto()
            {
                Id = request.Id,
                LanguageId = request.LanguageId,
                Key = request.Key,
                Value = request.Value
            };

            return await _managerLanguage.UpdateLanguageTextAsync(dto);
        }
    }
}
