using ScriptBuilder.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptBuilder.Query
{
    public class SelectQueryDefinition : QueryDefinition
	{
		private readonly List<QueryJoinInfo> _joins;
		private readonly List<FilterDefinition> _filters;

		public SelectQueryDefinition(string tableName)
		{
			_tableName = tableName;
			_filters = new List<FilterDefinition>();

			_values = new Dictionary<string, string>();
			_joins = new List<QueryJoinInfo>();
		}

		public SelectQueryDefinition All()
		{
			_values.Add("*", "*");
			return this;
		}

		public SelectQueryDefinition SelectField(string fieldName)
		{
			return SelectField(fieldName, null);
		}

		public SelectQueryDefinition SelectField(string fieldName, string aliasName)
		{
			_values.Add(fieldName, aliasName);
			return this;
		}

		public QueryJoinInfo Join(string tableName)
		{
			QueryJoinInfo join = new QueryJoinInfo(tableName);
			_joins.Add(join);

			return join;
		}

		public QueryJoinInfo Join(QueryJoinInfo join)
		{
			_joins.Add(join);

			return join;
		}

		public FilterDefinition Where(string fieldName)
		{
			FilterDefinition f = new FilterDefinition();
			f.Where(fieldName);
			f.AsGroup();

			_filters.Add(f);

			return f;
		}

		public FilterDefinition And(string fieldName)
		{
			FilterDefinition f = new FilterDefinition();
			f.Where(fieldName);
			f.AsGroup();

			f.FilterCondition = FilterLogicCondition.AND;

			_filters.Add(f);

			return f;
		}

		public FilterDefinition Or(string fieldName)
		{
			FilterDefinition f = new FilterDefinition();
			f.Where(fieldName);
			f.AsGroup();

			f.FilterCondition = FilterLogicCondition.OR;

			_filters.Add(f);

			return f;
		}

		public override string Render()
		{
			// Definir alias
			List<string> alias = new List<string>();
			alias.Add(_tableName);

			int c = 1;
			foreach (var t in _joins)
			{
				alias.Add(string.Format("{0}", t.Query._tableName));
				c++;
			}

			List<Dictionary<string, string>> allValues = new List<Dictionary<string, string>>();
			allValues.Add(_values);

			foreach (var j in _joins)
			{
				allValues.Add(j.Query._values);
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT ");

			RenderValues(ref sb, allValues, alias);

			sb.AppendFormat(" FROM {0} AS {1}", _tableName, alias[0]);

			RenderJoins(ref sb, alias);
			RenderFilters(ref sb, alias);			

			return sb.ToString();
		}

		private void RenderValues(ref StringBuilder sb, List<Dictionary<string, string>> allValues, List<string> alias)
		{
			int c = 0;
			foreach (var values in allValues)
			{
				if (values.Any())
				{
					// Si no campos definidos para la tabla actual, entonces se pone el alias y el asterisco
					// para seleccionar todos los campos de esa tabla
					sb.AppendFormat("{0}.*", alias[c]);
				}
				else
				{
					var d = 0;
					foreach (var v in values)
					{
						if (v.Key == "*")
						{
							// Si no campos definidos para la tabla actual, entonces se pone el alias y el asterisco
							// para seleccionar todos los campos de esa tabla
							sb.AppendFormat("{0}.*", alias[c]);
						}
						else
						{
							string al = v.Value != null && v.Value.Trim() != string.Empty ? v.Value : v.Key;
							sb.AppendFormat("{0}.{1} AS {2}", alias[c], v.Key, al);
						}

						if (d < values.Count - 1)
						{
							sb.Append(',');
						}

						d++;
					}
				}

				if (c < allValues.Count - 1)
				{
					sb.Append(',');
				}

				c++;
			}
		}

		private void RenderJoins(ref StringBuilder sb, List<string> alias)
		{
			// Render joins
			for (var jx = 0; jx < _joins.Count; jx++)
			{
				sb.AppendLine("");

				sb.Append(_joins[jx].Render(alias[jx + 1]));

				for (var cF = 0; cF < _joins[jx]._referencedFieldNames.Count; cF++)
				{
					if (cF == 0)
					{
						sb.Append(" ON ");
					}
					if (cF > 0)
					{
						sb.Append(" AND ");
					}

					sb.AppendFormat("{0}.{1} = {2}.{3}", alias[0], _joins[jx]._referencedFieldNames[cF], alias[jx + 1], _joins[jx]._fieldNames[cF]);
				}
			}
		}

		private void RenderFilters(ref StringBuilder sb, List<string> alias)
		{
			// Render joins
			if (_filters.Count > 0)
			{
				sb.Append(" WHERE ");

				foreach (var f in _filters)
				{
					f._aliasParentTableName = alias[0];
					sb.Append(f.Render());
				}
			}
		}
	}
}
