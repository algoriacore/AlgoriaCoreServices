using System.Text;

namespace ScriptBuilder.Filters
{
    public class FilterDefinition
	{
		private FilterDefinition _subFilter;
		private FilterOperators _operator;
		internal FilterLogicCondition FilterCondition;
		internal string _aliasParentTableName;

		public string _fieldName { get; set; }
		public string _fieldValue { get; set; }

		private string _condition;
		private bool _isGrouper = false;

		public FilterDefinition()
		{
			FilterCondition = FilterLogicCondition.NONE;
		}

		public FilterDefinition AsGroup()
		{
			_isGrouper = true;
			return this;
		}

		public FilterDefinition And(string fieldName)
		{
			_subFilter = new FilterDefinition();
			_subFilter.FilterCondition = FilterLogicCondition.AND;
			_subFilter._fieldName = fieldName;

			return _subFilter;
		}

		public FilterDefinition Or(string fieldName)
		{
			_subFilter = new FilterDefinition();
			_subFilter.FilterCondition = FilterLogicCondition.OR;
			_subFilter._fieldName = fieldName;

			return _subFilter;
		}

		public FilterDefinition Where(string fieldName)
		{
			_fieldName = fieldName;
			SetCondition();

			return this;
		}

		public FilterDefinition EqualsThan(string fieldValue)
		{
			_fieldValue = fieldValue;
			_operator = FilterOperators.Equal;
			SetCondition();

			return this;
		}

		public FilterDefinition GreatherThan(string fieldValue)
		{
			_fieldValue = fieldValue;
			_operator = FilterOperators.GreatherThan;
			SetCondition();

			return this;
		}

		public FilterDefinition GreatherOrEqualThan(string fieldValue)
		{
			_fieldValue = fieldValue;
			_operator = FilterOperators.GreatherOrEqualThan;
			SetCondition();

			return this;
		}

		public FilterDefinition LowerThan(string fieldValue)
		{
			_fieldValue = fieldValue;
			_operator = FilterOperators.LowerThan;
			SetCondition();

			return this;
		}

		public FilterDefinition LowerOrEqualThan(string fieldValue)
		{
			_fieldValue = fieldValue;
			_operator = FilterOperators.LowerOrEqualThan;
			SetCondition();

			return this;
		}

		public FilterDefinition Between(string rangeValue1, string rangeValue2)
		{
			_operator = FilterOperators.Between;

			_condition = string.Format("{0} BETWEEN '{1}' AND '{2}'", _fieldName, rangeValue1, rangeValue2);

			return this;
		}

		private void SetCondition()
		{
			if (_operator != FilterOperators.Between)
			{
				_condition = string.Format("{0}{1}'{2}'", _fieldName, GetOperatorString(), _fieldValue ?? string.Empty);
			}
		}

		public string Render()
		{
			StringBuilder sb = new StringBuilder();

			string _subfilterRender = string.Empty;
			if (_subFilter != null)
			{
				_subfilterRender = _subFilter.Render();
			}

			if (FilterCondition == FilterLogicCondition.NONE)
			{
				sb.AppendFormat("{0} {1} {2}", _isGrouper ? "(" : string.Empty, _condition, _subfilterRender);
			}
			else
			{
				sb.AppendFormat("{0} {1} {2} {3}", FilterCondition.ToString(), _isGrouper ? "(" : string.Empty  , _condition, _subfilterRender);
			}

			if (_isGrouper)
			{
				sb.Append(")");
			}

			return sb.ToString();
		}

		private string GetOperatorString()
		{
			string op = string.Empty;
			switch (_operator)
			{
				case FilterOperators.Equal:
					op = "=";
					break;
				case FilterOperators.GreatherOrEqualThan:
					op = ">=";
					break;
				case FilterOperators.GreatherThan:
					op = ">";
					break;
				case FilterOperators.LowerOrEqualThan:
					op = "<=";
					break;
				case FilterOperators.LowerThan:
					op = "<";
					break;
				case FilterOperators.Between:
					op = "BETWEEN";
					break;
			}

			return op;
		}
	}
}
