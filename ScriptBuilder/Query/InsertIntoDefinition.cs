using System.Collections.Generic;
using System.Text;

namespace ScriptBuilder.Query
{
    public class InsertIntoDefinition : QueryDefinition
	{
		public InsertIntoDefinition(string tableName)
		{
			_tableName = tableName;
			_values = new Dictionary<string, string>();
		}

		public void AddField(string fieldName, string fieldValue)
		{
			_values.Add(fieldName, fieldValue);
		}

		public override string Render()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("INSERT INTO {0} (", _tableName);

			int c = 1;
			foreach(var d in _values)
			{
				sb.AppendFormat("{0}", d.Key);

				if(c != _values.Count)
				{
					sb.Append(",");
				}
				c++;
			}

			sb.Append(") VALUES (");

			c = 1;
			foreach (var d in _values)
			{
				sb.AppendFormat("'{0}'", d.Value);

				if (c != _values.Count)
				{
					sb.Append(",");
				}
				c++;
			}

			sb.Append(");");

			return sb.ToString();
		}
	}
}
