namespace ScriptBuilder.Structure.Fields.Numerics
{
    public class BigIntFieldDefinition : FieldDefinition
	{
		internal BigIntFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			_dataType = "BIGINT";
			_parentTableName = parentTableName;
		}

		public BigIntFieldDefinition AsIdentity()
		{
			return AsIdentity(1, 1);
		}

		public BigIntFieldDefinition AsIdentity(int seed)
		{
			return AsIdentity(seed, 1);
		}

		public BigIntFieldDefinition AsIdentity(int seed, int increment)
		{
			_dataType = string.Format("BIGINT IDENTITY({0},{1})", seed, increment);
			return this;
		}
	}
}
