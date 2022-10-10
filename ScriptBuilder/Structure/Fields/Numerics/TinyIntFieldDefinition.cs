namespace ScriptBuilder.Structure.Fields.Numerics
{
    public class TinyIntFieldDefinition : FieldDefinition
	{
		internal TinyIntFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "TINYINT";
			_parentTableName = parentTableName;
		}		
	}
}
