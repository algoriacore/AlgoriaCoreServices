using System.Collections.Generic;
using System.Text;

namespace ScriptBuilder.Query
{
    public class QueryBuilder
	{
		private readonly List<QueryDefinition> _queries;

		public QueryBuilder()
		{
			_queries = new List<QueryDefinition>();
		}

		public SelectQueryDefinition Select(string tableName)
		{
			var s = new SelectQueryDefinition(tableName);

			_queries.Add(s);

			return s;
		}

		public InsertIntoDefinition InsertInto(string tableName)
		{
			var s = new InsertIntoDefinition(tableName);

			_queries.Add(s);

			return s;
		}

		public UpdateQueryDefinition Update(string tableName)
		{
			var s = new UpdateQueryDefinition(tableName);

			_queries.Add(s);

			return s;
		}

		public string GenerateScripts()
		{
			StringBuilder sb = new StringBuilder();

			foreach(var q in _queries)
			{
				sb.AppendLine(q.Render());
				sb.AppendLine(";");
				sb.AppendLine("");
			}

			return sb.ToString();
		}
	}
}
