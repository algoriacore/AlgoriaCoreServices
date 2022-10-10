using System.Threading.Tasks;

namespace AlgoriaCore.Application.Tests.ASanitization
{
    public interface IASanitizeRuleDefinition
    {
        bool Succcess { get; set; }
        string MessageResult { get; set; }
        Task Run();
    }
}
