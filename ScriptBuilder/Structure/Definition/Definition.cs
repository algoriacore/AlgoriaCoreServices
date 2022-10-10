using System;

namespace ScriptBuilder.Structure.Definition
{
    public abstract class Definition //: IDefinition
	{
		protected string _name;

		internal DefinitionTypes DefinitionType { get; set; }
		internal int Position { get; set; }

		internal virtual string Render()
		{
			throw new NotImplementedException();
		}
	}

	public enum DefinitionTypes
	{
		Field = 1,
		Constraint = 2
	}
}
