namespace ScriptBuilder.Structure.Constraints
{
    internal class ForeignKeyDefinition : ConstraintDefinition
	{
		private string _sourceFieldsName;
		private string _referencedTableName;
		private string _referencedFieldsName;

		private bool _toDrop;

		public ForeignKeyDefinition(string name, string tableName) : base()
		{
			_name = name;
			_ownerTableName = tableName;
			_constraintType = ConstraintTypes.ForeignKey; 
		}

		public ForeignKeyDefinition Drop()
		{
			_toDrop = true;
			return this;
		}

		public ForeignKeyDefinition UsingSourceFields(string sourceFields)
		{
			_sourceFieldsName = sourceFields;
			return this;
		}

		public ForeignKeyDefinition ReferencingTable(string refTableName)
		{
			_referencedTableName = refTableName;
			return this;
		}

		public ForeignKeyDefinition UsingReferencedFields(string refFieldsName)
		{
			_referencedFieldsName = refFieldsName;
			return this;
		}

		internal override string Render()
		{
			if (_toDrop)
			{
				return string.Format("ALTER TABLE [{0}] DROP CONSTRAINT {1}", _ownerTableName, _name);
			}
			else
			{
				return string.Format("ALTER TABLE [{0}] ADD CONSTRAINT {1}\n\tFOREIGN KEY ([{2}]) REFERENCES [{3}]([{4}])", _ownerTableName, _name, _sourceFieldsName, _referencedTableName, _referencedFieldsName);
			}
		}
	}
}
