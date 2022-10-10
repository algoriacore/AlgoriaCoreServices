using ScriptBuilder.Filters;
using System.Collections.Generic;
using System.Text;

namespace ScriptBuilder.Query
{
    public class UpdateQueryDefinition : QueryDefinition
	{
		private readonly List<FilterDefinition> _filters;

		public UpdateQueryDefinition(string tableName)
		{
			_tableName = tableName;
			_values = new Dictionary<string, string>();
			_filters = new List<FilterDefinition>();
		}

		public UpdateQueryDefinition AddField(string fieldName, string fieldValue)
		{
			_values.Add(fieldName, fieldValue);

			return this;
		}

		public FilterDefinition Where(string fieldName)
		{
			FilterDefinition f = new FilterDefinition();
			f.Where(fieldName);

			_filters.Add(f);

			return f;
		}

		public FilterDefinition And(string fieldName)
		{
			FilterDefinition f = new FilterDefinition();
			f.Where(fieldName);
			f.FilterCondition = FilterLogicCondition.AND;

			_filters.Add(f);

			return f;
		}

		public FilterDefinition Or(string fieldName)
		{
			FilterDefinition f = new FilterDefinition();
			f.Where(fieldName);
			f.FilterCondition = FilterLogicCondition.OR;

			_filters.Add(f);

			return f;
		}

		public override string Render()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("UPDATE {0} SET ", _tableName);

			int c = 1;
			foreach (var d in _values)
			{
				sb.AppendFormat("{0}='{1}'", d.Key, d.Value);

				if (c != _values.Count)
				{
					sb.Append(",");
				}
				c++;
			}

			// Render filters
			if (_filters.Count > 0)
			{
				sb.Append(" WHERE ");

				foreach (var f in _filters)
				{
					sb.Append(f.Render());
				}
			}

			return sb.ToString();
		}
	}
}
