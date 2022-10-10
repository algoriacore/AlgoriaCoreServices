namespace ScriptBuilder.Structure.Fields.Binaries
{
    public class VarBinaryFieldDefinition : FieldDefinition
	{
		internal VarBinaryFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "VARBINARY(MAX)";
			_parentTableName = parentTableName;
		}

		public VarBinaryFieldDefinition WithLength(int length)
		{
			_dataType = string.Format("VARBINARY({0})", length);
			return this;
		}

		public VarBinaryFieldDefinition WithMaxLength()
		{
			_dataType = "VARBINARY(MAX)";
			return this;
		}
	}
}
