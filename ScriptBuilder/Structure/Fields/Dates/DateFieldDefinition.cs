namespace ScriptBuilder.Structure.Fields.Dates
{
    public class DateFieldDefinition : FieldDefinition
	{
		internal DateFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "DATE";
			_parentTableName = parentTableName;
		}		
	}
}
