namespace ScriptBuilder.Structure.Fields.Numerics
{
    public class SmallIntFieldDefinition : FieldDefinition
	{
		internal SmallIntFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "SMALLINT";
			_parentTableName = parentTableName;
		}		
	}
}
