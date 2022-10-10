namespace ScriptBuilder.Structure.Fields.Dates
{
    public class DateTimeFieldDefinition : FieldDefinition
	{
		internal DateTimeFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "DATETIME";
			_parentTableName = parentTableName;
		}		
	}
}
