using ScriptBuilder.Structure.Definition;
using System.Text;

namespace ScriptBuilder.Structure.Fields
{
    public abstract class FieldDefinition: Definition.Definition
	{
		protected string _parentTableName;
		protected string _dataType;
		private bool _isPrimaryKey = false;
		private string _primaryKeyName;
		private bool _notNull = false;

		private bool _isForeignKey;
		private string _fkName;
		private string _fkTableName;
		private string _fkFieldName;

		private bool _isDropped;

        internal bool IsNew { get; set; }
        internal bool IsInNewTable { get; set; }

		protected FieldDefinition()
		{
			DefinitionType = DefinitionTypes.Field;
		}

		public FieldDefinition AsNotNull()
		{
			_notNull = true;
			return this;
		}

		#region Primary key

		public FieldDefinition AsPrimaryKey()
		{
			_isPrimaryKey = true;
			return this;
		}

		public void AsPrimaryKey(string pkName)
		{
			_primaryKeyName = pkName;
			_isPrimaryKey = true;
		}

		internal bool IsPrimaryKey()
		{
			return _isPrimaryKey;
		}

		internal string PrimaryKeyName
		{
			get
			{
				return _primaryKeyName;
			}
		}

		#endregion

		#region Foreign Key

		internal bool IsForeignKey()
		{
			return _isForeignKey;
		}

		internal string ForeigKeyName
		{
			get
			{
				return _fkName;
			}
		}

		internal string ForeignKeyReferencedTableName
		{
			get
			{
				return _fkTableName;
			}
		}

		internal string ForeignKeyReferencedFieldName
		{
			get
			{
				return _fkFieldName;
			}
		}

		public FieldDefinition AsForeignKey(string refTAbleName, string refFieldName)
		{
			return AsForeignKey(null, refTAbleName, refFieldName);
		}

		public FieldDefinition AsForeignKey(string fkName, string refTAbleName, string refFieldName)
		{
			_isForeignKey = true;
			_fkName = fkName;
			_fkTableName = refTAbleName;
			_fkFieldName = refFieldName;

			return this;
		}

		#endregion

		#region Drop field
	
		public FieldDefinition Drop()
		{
			_isDropped = true;
			return this;
		}

		#endregion

		public string Name
		{
			get
			{
				return _name;
			}
		}

		internal override string Render()
		{
			StringBuilder sb = new StringBuilder();

			if (IsInNewTable)
			{
				sb.AppendFormat("[{0}] {1} {2}", _name, _dataType, _notNull ? "NOT NULL" : string.Empty);
			}
			else
			{
				if (_isDropped)
				{
					sb.AppendFormat("ALTER TABLE [{0}] DROP COLUMN [{1}]", _parentTableName, _name);
				}
                else if (IsNew)
                {
                    sb.AppendFormat("ALTER TABLE [{0}] ADD [{1}] {2} {3}", _parentTableName, _name, _dataType, _notNull ? "NOT NULL" : string.Empty);
                }
				else
				{
					sb.AppendFormat("ALTER TABLE [{0}] ALTER COLUMN [{1}] {2} {3}", _parentTableName, _name, _dataType, _notNull ? "NOT NULL" : string.Empty);
				}
			}

			return sb.ToString();
		}
	}
}
