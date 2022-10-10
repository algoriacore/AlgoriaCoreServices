using System.Data.Common;
using System.Data.SqlClient;

namespace ScriptBuilder.Database
{
    public class ScriptExecuter
	{
		private readonly string _connectionString;
		private SqlConnection _cnn;

		public ScriptExecuter(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void Execute(string script)
		{
			OpenConnection();

			using (DbCommand command = new SqlCommand(script))
			{
				command.Connection = _cnn;
				command.ExecuteNonQuery();
			}

			CloseConnection();
		}

		private void OpenConnection()
		{
			if(_cnn == null || _cnn.State == System.Data.ConnectionState.Closed)
			{
				_cnn = new SqlConnection(_connectionString);
				_cnn.Open();
			}
		}

		private void CloseConnection()
		{
			if(_cnn != null)
			{
				_cnn.Close();
			}
		}
	}
}
