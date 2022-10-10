namespace ScriptBuilder.Structure.Fields.Numerics
{
    public class DecimalFieldDefinition : FieldDefinition
	{
		int _precision = 18;
		int _decimals = 6;

		internal DecimalFieldDefinition(string fieldName, string parentTableName)
		{
			_name = fieldName;
			SetDataType(); // Precisión default
			_parentTableName = parentTableName;
		}

		public DecimalFieldDefinition SetPrecision(int precision)
		{
			_precision = precision;
			SetDataType();

			return this;
		}

		public DecimalFieldDefinition SetDecimals(int decimals)
		{
			_decimals = decimals;
			SetDataType();

			return this;
		}

		private void SetDataType()
		{
			_dataType = string.Format("DECIMAL({0},{1})", _precision, _decimals);
		}
	}
}
