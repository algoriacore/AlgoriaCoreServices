namespace ScriptBuilder.Structure.Fields.Texts
{
    public class CharFieldDefinition : FieldDefinition
	{
		internal CharFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "CHAR";
			_parentTableName = parentTableName;
		}

		public CharFieldDefinition WithLength(int length)
		{
			_dataType = string.Format("CHAR({0})", length);
			return this;
		}

		public CharFieldDefinition WithMaxLength()
		{
			_dataType = "CHAR(MAX)";
			return this;
		}
	}
}
