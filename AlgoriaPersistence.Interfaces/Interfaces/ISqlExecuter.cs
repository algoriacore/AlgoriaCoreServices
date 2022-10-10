using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaPersistence.Interfaces.Interfaces
{
    public interface ISqlExecuter
    {
        Task<int> ExecuteSqlCommandAsync(string aSql, params object[] aParameters);
        Task<int> ExecuteSqlCommandAsync(string aSql, Dictionary<string, object> aParameters = null);
        Task<List<Dictionary<string, object>>> SqlQueryToDictionary(string aSql, Dictionary<string, object> aParameters = null);
        Task<List<Dictionary<string, object>>> SqlStoredProcedureToDictionary(string aSql, Dictionary<string, object> aParameters = null);
        Task<List<T>> SqlQuery<T>(string aSql, Dictionary<string, object> aParameters = null);
        Task<List<T>> SqlStoredProcedure<T>(string aSql, Dictionary<string, object> aParameters = null);
        Task<T> ExecuteSqlQueryScalar<T>(string aSql, Dictionary<string, object> aParameters = null);
        Task<T> ExecuteStoredProcedureScalar<T>(string aSql, Dictionary<string, object> aParameters = null);
    }
}