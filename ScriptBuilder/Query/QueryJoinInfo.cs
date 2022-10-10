using System.Collections.Generic;
using System.Text;

namespace ScriptBuilder.Query
{
    public class QueryJoinInfo
	{
		private readonly string _tableName;
		internal List<string> _referencedFieldNames;
		internal List<string> _fieldNames;

		private readonly SelectQueryDefinition _queryDefinition;

		public QueryJoinInfo(string tableName)
		{
			_tableName = tableName;

			_referencedFieldNames = new List<string>();
			_fieldNames = new List<string>();

			_queryDefinition = new SelectQueryDefinition(tableName);
		}

		public SelectQueryDefinition Query
		{
			get
			{
				return _queryDefinition;
			}
		}

		public QueryJoinInfo On(string referencedFieldName, string fieldName)
		{
			_referencedFieldNames.Add(referencedFieldName);
			_fieldNames.Add(fieldName);

			return this;
		}

		public QueryJoinInfo SelectField(string field)
		{
			return SelectField(field, null);
		}

		public QueryJoinInfo SelectField(string field, string alias)
		{
			_queryDefinition.SelectField(field, alias);
			return this;
		}

		internal string Render(string alias)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("INNER JOIN ({0}) AS {1} ", _queryDefinition.Render(), alias);

			return sb.ToString();
		}

		public string TableName
		{
			get
			{
				return _tableName;
			}
		}
	}
}
