namespace ScriptBuilder.Structure.Fields.Numerics
{
    public class NoTypeFieldDefinition : FieldDefinition
	{
		internal NoTypeFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "";
			_parentTableName = parentTableName;
		}
	}
}
