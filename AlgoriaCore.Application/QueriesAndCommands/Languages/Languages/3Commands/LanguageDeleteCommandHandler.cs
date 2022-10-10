using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageDeleteCommandHandler : BaseCoreClass, IRequestHandler<LanguageDeleteCommand, int>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageDeleteCommandHandler(ICoreServices coreServices
        , LanguageManager managerLanguage): base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<int> Handle(LanguageDeleteCommand request, CancellationToken cancellationToken)
        {
            await _managerLanguage.DeleteLanguageAsync(request.Id);

            return request.Id;
        }
    }
}
