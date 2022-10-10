namespace ScriptBuilder.Structure.Fields.Dates
{
    public class TimeFieldDefinition : FieldDefinition
	{
		internal TimeFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "TIME";
			_parentTableName = parentTableName;
		}		
	}
}
