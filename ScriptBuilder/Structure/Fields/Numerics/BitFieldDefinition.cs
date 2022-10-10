namespace ScriptBuilder.Structure.Fields.Numerics
{
    public class BitFieldDefinition : FieldDefinition
	{
		internal BitFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "BIT";
			_parentTableName = parentTableName;
		}		
	}
}
