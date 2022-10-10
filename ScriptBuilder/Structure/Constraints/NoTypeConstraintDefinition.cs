namespace ScriptBuilder.Structure.Constraints
{
    internal class NoTypeConstraintDefinition : ConstraintDefinition
	{
		public NoTypeConstraintDefinition(string name) : base()
		{
			_name = name;
			_constraintType = ConstraintTypes.NoType;
		}
	}
}
