using ScriptBuilder.Database;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptBuilder.Structure
{
    public class ScriptBuilder
	{
		public List<TableDefinition> _tablesDefinitions { get; set; }

		public ScriptBuilder()
		{
			_tablesDefinitions = new List<TableDefinition>();
		}

		public TableDefinition AddTableDefinition(string tableName)
		{
			var tb = new TableDefinition(tableName);

			_tablesDefinitions.Add(tb);

			return tb;
		}

		private List<OrderedSentence> GetOrderedSentences()
		{
			List<OrderedSentence> bigList = new List<OrderedSentence>();

			foreach (var t in _tablesDefinitions)
			{
				bigList.AddRange(t.GetScripts());
			}

			return bigList;
		}

		public string GenerateScripts()
		{
			var bigList = GetOrderedSentences();

			StringBuilder sb = new StringBuilder();

			foreach (var s in bigList.OrderBy(m => m.Order))
			{
				sb.AppendLine(s.Sentence);
				sb.AppendLine(";");
				sb.AppendLine("");
			}

			return sb.ToString();
		}

		public void ExecuteScripts(string connectionString)
		{
			var scripts = GenerateScripts();

			ScriptExecuter ex = new ScriptExecuter(connectionString);
			ex.Execute(scripts);
		}		
	}
}
