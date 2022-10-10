using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextCreateCommandHandler : BaseCoreClass, IRequestHandler<LanguageTextCreateCommand, long>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageTextCreateCommandHandler(ICoreServices coreServices
        , LanguageManager managerLanguage): base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<long> Handle(LanguageTextCreateCommand request, CancellationToken cancellationToken)
        {
            LanguageTextDto dto = new LanguageTextDto()
            {
                LanguageId = request.LanguageId,
                Key = request.Key,
                Value = request.Value
            };

            return await _managerLanguage.CreateLanguageTextAsync(dto);
        }
    }
}
