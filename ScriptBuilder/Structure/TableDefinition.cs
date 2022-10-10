using ScriptBuilder.Structure.Constraints;
using ScriptBuilder.Structure.Definition;
using ScriptBuilder.Structure.Fields;
using ScriptBuilder.Structure.Fields.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptBuilder.Structure
{
    public class TableDefinition
	{
		private int _counter;
		private readonly string _tableName;
		private bool _isNewTable;

		private readonly List<Definition.Definition> _newFieldsDefinitions;
		private readonly List<Definition.Definition> _altersDefinition;

		public TableDefinition(string tableName)
		{
			_tableName = tableName;
			_newFieldsDefinitions = new List<Definition.Definition>();
			_altersDefinition = new List<Definition.Definition>();
		}

		public T AddField<T>(string fieldName)
			where T : FieldDefinition
		{
			_counter++;

			var ctor = typeof(T).GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

			object[] pars = new object[] { fieldName, _tableName };
			var t = (T)(ctor[0].Invoke(pars));
			t.Position = _counter;
            _newFieldsDefinitions.Add(t);

			return t;
		}

		public T ChangeField<T>(string fieldName)
			where T : FieldDefinition
		{
			_counter++;

			var ctor = typeof(T).GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

			object[] pars = new object[] { fieldName, _tableName };
			var t = (T)(ctor[0].Invoke(pars));
			t.Position = _counter;
			_altersDefinition.Add(t);

			return t;
		}

		public FieldDefinition DropField(string fieldName)
		{
			_counter++;

			var t = new NoTypeFieldDefinition(fieldName, _tableName);
			t.Position = _counter;
			t.Drop();
			
			_altersDefinition.Add(t);

			return t;
		}

        public ConstraintDefinition AddPrimaryKey(string name)
		{
			_counter++;

			var t = new PrimaryKeyDefinition(name, _tableName);
			t.Position = _counter;

			_altersDefinition.Add(t);
			return t;
		}

		public ConstraintDefinition AddForeignKey(string name)
		{
			_counter++;

			var t = new ForeignKeyDefinition(name, _tableName);
			t.Position = _counter;

			_altersDefinition.Add(t);
			return t;
		}

		public ConstraintDefinition AddUnique(string name)
		{
			_counter++;

			var t = new UniqueDefinition(name, _tableName);
			t.Position = _counter;

			_altersDefinition.Add(t);
			return t;
		}

		public ConstraintDefinition DropForeignKey(string fieldName, string refTAbleName, string refFieldName)
		{
			_counter++;

			var t = new ForeignKeyDefinition(string.Format("FK_{0}_{1}", _tableName, refTAbleName), _tableName);

			t.Position = _counter;
			t.UsingSourceFields(fieldName);
			t.ReferencingTable(refTAbleName);
			t.UsingReferencedFields(refFieldName);

			t.Drop();

			_altersDefinition.Add(t);

			return t;
		}

		public TableDefinition AsNewTable()
		{
			_isNewTable = true;
			return this;
		}

		internal List<OrderedSentence> GetScripts()
		{
			List<OrderedSentence> resps = new List<OrderedSentence>();

			// Si se está creando la tabla, entonces se renderiza como una instrucción CREATE TABLE
			// y se agregan todos los campos registrados con el método "AddField"
			if (_isNewTable)
			{
				StringBuilder sbPattern = new StringBuilder();
				sbPattern.AppendLine(string.Format("CREATE TABLE [{0}] (", _tableName));

				var c = 0;
				foreach (var f in _newFieldsDefinitions)
				{
					c++;

					var fa = (FieldDefinition)f;
					fa.IsInNewTable = true;

					sbPattern.Append("\t" + fa.Render());

					if (c < _newFieldsDefinitions.Count)
					{
						sbPattern.Append(",");
					}

					sbPattern.AppendLine();
				}

				sbPattern.AppendLine(")");

				resps.Add(new OrderedSentence { Order = 0, Sentence = sbPattern.ToString() });
			}

			// Ahora se crean el resto de scripts ordenados de forma ascendente
			RenderScripts(ref resps);

			return resps;
		}

		private void RenderScripts(ref List<OrderedSentence> resps)
		{
			for (var i = 1; i <= _counter; i++)
			{
				if (!_isNewTable)
				{
					var rActual = _newFieldsDefinitions.FirstOrDefault(m => m.Position == i);
					if (rActual != null)
					{
						var f = (FieldDefinition)rActual;
                        f.IsNew = true;

						resps.Add(new OrderedSentence { Order = i, Sentence = f.Render() });
					}
				}

				// Se crean los Primary keys definidos directamente en los campos de la tabla
				CreatePKs(ref resps, i);

				// Se crean los Foreign keys definidos directamente en los campos de la tabla
				CreateFKs(ref resps, i);

				// Ahora se busca en la lista de "alters"
				var cActual = _altersDefinition.FirstOrDefault(m => m.Position == i);
				if (cActual != null)
				{
					var cA = cActual;

					// Si la definición actual es tipo "Field
					if (cA.DefinitionType == DefinitionTypes.Field)
					{
						var f = (FieldDefinition)cActual;

						resps.Add(new OrderedSentence { Order = i, Sentence = f.Render() });
					}
					// Si es de tipo "Constraint"
					else if (cA.DefinitionType == DefinitionTypes.Constraint)
					{
						var c = (ConstraintDefinition)cActual;
						resps.Add(new OrderedSentence { Order = i, Sentence = c.Render() });
					}
				}
			}
		}

		private void CreatePKs(ref List<OrderedSentence> resps, int i)
		{
			var pkAct = _newFieldsDefinitions.FirstOrDefault(m => m.Position == i);
			if (pkAct != null && pkAct.DefinitionType == DefinitionTypes.Field)
			{
				var cAct = (FieldDefinition)pkAct;
				if (cAct.IsPrimaryKey())
				{
					// Se crea manualmente el Primary Key
					var pk = new PrimaryKeyDefinition(cAct.PrimaryKeyName ?? string.Format("PK_{0}", _tableName), _tableName).UsingSourceFields(cAct.Name);
					resps.Add(new OrderedSentence { Order = i, Sentence = pk.Render() });
				}
			}
		}

		private void CreateFKs(ref List<OrderedSentence> resps, int i)
		{
			var fkAct = _newFieldsDefinitions.FirstOrDefault(m => m.Position == i);
			if (fkAct != null && fkAct.DefinitionType == DefinitionTypes.Field)
			{
				var cAct = (FieldDefinition)fkAct;
				if (cAct.IsForeignKey())
				{
					// Se crea manualmente el Primary Key
					var fk = new ForeignKeyDefinition(cAct.ForeigKeyName ?? string.Format("FK_{0}_{1}", _tableName, cAct.ForeignKeyReferencedTableName), _tableName)
								.UsingSourceFields(cAct.Name).ReferencingTable(cAct.ForeignKeyReferencedTableName).UsingReferencedFields(cAct.ForeignKeyReferencedFieldName);

					resps.Add(new OrderedSentence { Order = i, Sentence = fk.Render() });
				}
			}
		}
	}
}
