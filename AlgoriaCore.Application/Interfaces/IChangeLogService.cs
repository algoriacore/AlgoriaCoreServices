using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Interfaces
{
    public interface IChangeLogService
    {
        Task<long> LogChange(ChangeLogType changeLogType, string key, string table, string detail);
    }
}
