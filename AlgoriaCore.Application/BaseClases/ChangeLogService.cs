using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChangeLogs;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using Autofac;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Localization
{
    public class ChangeLogService : IChangeLogService
    {
        private ChangeLogManager _managerChangeLog;
        private readonly ILifetimeScope _lifetimeScope;

        public ChangeLogService(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public async Task<long> LogChange(ChangeLogType changeLogType, string key, string table, string detail)
        {
            SetChangeLogManager();

            ChangeLogDto dto = new ChangeLogDto();
            dto.Key = key;
            dto.Table = table;

            switch (changeLogType)
            {
                case ChangeLogType.Delete:
                    dto.Detail = "{{RecordDeleted}}";
                    break;
                case ChangeLogType.Create:
                    dto.Detail = "{{RecordCreated}}" + "\n" + detail;
                    break;
                case ChangeLogType.Update:
                    dto.Detail = "{{RecordUpdated}}" + "\n" + detail;
                    break;
                case ChangeLogType.SoftDelete:
                    dto.Detail = "{{RecordSoftDeleted}}";
                    break;
            }

            return await _managerChangeLog.CreateChangeLogAsync(dto);
        }

        private void SetChangeLogManager()
        {
            if (_managerChangeLog == null)
            {
                _managerChangeLog = _lifetimeScope.Resolve<ChangeLogManager>();
            }
        }
    }
}
