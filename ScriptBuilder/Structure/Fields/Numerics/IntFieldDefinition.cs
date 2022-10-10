namespace ScriptBuilder.Structure.Fields.Numerics
{
    public class IntFieldDefinition : FieldDefinition
	{
		internal IntFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "INT";
			_parentTableName = parentTableName;
		}

		public IntFieldDefinition AsIdentity()
		{
			return AsIdentity(1, 1);
		}

		public IntFieldDefinition AsIdentity(int seed)
		{
			return AsIdentity(seed, 1);
		}

		public IntFieldDefinition AsIdentity(int seed, int increment)
		{
			_dataType = string.Format("INT IDENTITY({0},{1})", seed, increment);
			return this;
		}
	}
}
