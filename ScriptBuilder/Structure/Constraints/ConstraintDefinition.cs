using ScriptBuilder.Structure.Definition;

namespace ScriptBuilder.Structure.Constraints
{
    public abstract class ConstraintDefinition : Definition.Definition
	{
		internal ConstraintTypes _constraintType;
		protected string _ownerTableName;

		protected ConstraintDefinition()
		{
			DefinitionType = DefinitionTypes.Constraint;
		}
	}

	internal enum ConstraintTypes
	{
		NoType,
		PrimaryKey,
		ForeignKey,
		Unique
	}
}
