namespace ScriptBuilder.Structure.Fields.Binaries
{
    public class BinaryFieldDefinition : FieldDefinition
	{
		internal BinaryFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;			
			_dataType = "BINARY(100)";
			_parentTableName = parentTableName;
		}

		public BinaryFieldDefinition WithLength(int length)
		{
			_dataType = string.Format("BINARY({0})", length);
			return this;
		}

		public BinaryFieldDefinition WithMaxLength()
		{
			_dataType = "VARBINARY(MAX)";
			return this;
		}
	}
}
