namespace ScriptBuilder.Structure.Fields.Numerics
{
    public class FloatFieldDefinition : FieldDefinition
	{
		int _precision = 7;
		int _decimals = 4;

		internal FloatFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			SetDataType(); // Precisión default
			_parentTableName = parentTableName;
		}

		public FloatFieldDefinition SetPrecision(int precision)
		{
			_precision = precision;
			SetDataType();

			return this;
		}

		public FloatFieldDefinition SetDecimals(int decimals)
		{
			_decimals = decimals;
			SetDataType();

			return this;
		}

		private void SetDataType()
		{
			_dataType = string.Format("FLOAT({0},{1})", _precision, _decimals);
		}
	}
}
