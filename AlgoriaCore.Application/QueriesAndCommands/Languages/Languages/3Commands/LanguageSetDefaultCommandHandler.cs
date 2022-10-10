using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageSetDefaultCommandHandler : BaseCoreClass, IRequestHandler<LanguageSetDefaultCommand, int>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageSetDefaultCommandHandler(ICoreServices coreServices
        , LanguageManager managerLanguage): base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<int> Handle(LanguageSetDefaultCommand request, CancellationToken cancellationToken)
        {
            _managerLanguage.SetLanguageDefault(request.Language);

            return await Task.FromResult(request.Language);
        }
    }
}
