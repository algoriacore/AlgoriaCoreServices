namespace ScriptBuilder.Structure.Fields.Texts
{
    public class VarCharFieldDefinition : FieldDefinition
	{
		internal VarCharFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "VARCHAR";
			_parentTableName = parentTableName;
		}

		public VarCharFieldDefinition WithLength(int length)
		{
			_dataType = string.Format("VARCHAR({0})", length);
			return this;
		}

		public VarCharFieldDefinition WithMaxLength()
		{
			_dataType = "VARCHAR(MAX)";
			return this;
		}
	}
}
