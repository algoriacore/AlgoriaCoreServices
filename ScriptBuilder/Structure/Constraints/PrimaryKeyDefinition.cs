namespace ScriptBuilder.Structure.Constraints
{
    internal class PrimaryKeyDefinition : ConstraintDefinition
	{
		private string _sourceFieldsName;

		private bool _toDrop;

		public PrimaryKeyDefinition(string name, string tableName) : base()
		{
			_name = name;
			_ownerTableName = tableName;
			_constraintType = ConstraintTypes.PrimaryKey;
		}

		public PrimaryKeyDefinition Drop()
		{
			_toDrop = true;
			return this;
		}

		public PrimaryKeyDefinition UsingSourceFields(string sourceFields)
		{
			_sourceFieldsName = sourceFields;
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
				return string.Format("ALTER TABLE {0} ADD CONSTRAINT {1}\n\tPRIMARY KEY ({2})", _ownerTableName, _name, _sourceFieldsName);
			}
		}
	}
}
