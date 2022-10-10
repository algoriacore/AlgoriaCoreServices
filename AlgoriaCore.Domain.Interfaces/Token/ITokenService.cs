using System.Threading.Tasks;

namespace AlgoriaCore.Domain.Interfaces.Token
{
    public interface ITokenService
    {
        Task<string> GetToken(bool force = false);
    }
}
