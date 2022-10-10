namespace ScriptBuilder.Interfaces
{
    public interface IFieldDefinition
	{
		string Name { get; }
		bool IsPrimaryKey();
		string PrimaryKeyName { get; }
		string ToSqlString();
	}
}
