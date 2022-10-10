using System.Collections.Generic;

namespace ScriptBuilder.Query
{
    public abstract class QueryDefinition
	{
		protected string _tableName;
		protected Dictionary<string, string> _values;

		public virtual string Render()
		{
			return string.Empty;
		}
	}
}
