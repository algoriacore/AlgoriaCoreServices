namespace ScriptBuilder.Structure.Constraints
{
    internal class UniqueDefinition : ConstraintDefinition
	{
		private string _sourceFieldsName;
		private string _referencedTableName;
		private string _referencedFieldsName;

		private bool _toDrop;

		public UniqueDefinition(string name, string tableName) : base()
		{
			_name = name;
			_ownerTableName = tableName;
			_constraintType = ConstraintTypes.Unique;
		}

		public UniqueDefinition Drop()
		{
			_toDrop = true;
			return this;
		}

		public UniqueDefinition UsingSourceFields(string sourceFields)
		{
			_sourceFieldsName = sourceFields;
			return this;
		}

		public UniqueDefinition ReferencingTable(string refTableName)
		{
			_referencedTableName = refTableName;
			return this;
		}

		public UniqueDefinition UsingReferencedFields(string refFieldsName)
		{
			_referencedFieldsName = refFieldsName;
			return this;
		}

		internal override string Render()
		{
			if (_toDrop)
			{
				return string.Format("ALTER TABLE {0} DROP CONSTRAINT {1}", _ownerTableName, _name);
			}
			else
			{
				return string.Format("ALTER TABLE {0} ADD CONSTRAINT {1}\n\tUNIQUE ({2}) REFERENCES {3}({4})", _ownerTableName, _name, _sourceFieldsName, _referencedTableName, _referencedFieldsName);
			}
		}
	}
}
