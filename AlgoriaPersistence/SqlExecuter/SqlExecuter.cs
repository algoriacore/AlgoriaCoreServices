using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AlgoriaPersistence.SqlExecuter
{
    public class SqlExecuter : ISqlExecuter
    {
        internal AlgoriaCoreDbContext Context;

        public SqlExecuter(AlgoriaCoreDbContext context)
        {
            Context = context;
        }

        public async Task<int> ExecuteSqlCommandAsync(string aSql, params object[] aParameters)
        {
            return await Context.Database.ExecuteSqlRawAsync(aSql, aParameters);
        }

        public async Task<int> ExecuteSqlCommandAsync(string aSql, Dictionary<string, object> aParameters = null)
        {
            List<SqlParameter> paramObjects = new List<SqlParameter>();

            if (aParameters != null)
            {
                foreach (var param in aParameters)
                {
                    var p = new SqlParameter
                    {
                        ParameterName = param.Key,
                        Value = param.Value == null ? DBNull.Value : param.Value
                    };

                    paramObjects.Add(p);
                }
            }

            return await Context.Database.ExecuteSqlRawAsync(aSql, paramObjects);
        }

        public async Task<List<Dictionary<string, object>>> SqlQueryToDictionary(string aSql, Dictionary<string, object> aParameters = null) {
            return await ExecuteSqlQueryToDictionary(CommandType.Text, aSql, aParameters);
        }

        public async Task<List<T>> SqlQuery<T>(string aSql, Dictionary<string, object> aParameters = null)
        {
            return await ExecuteSqlQuery<T>(CommandType.Text, aSql, aParameters);
        }

        public async Task<List<Dictionary<string, object>>> SqlStoredProcedureToDictionary(string aSql, Dictionary<string, object> aParameters = null) {
            return await ExecuteSqlQueryToDictionary(CommandType.StoredProcedure, aSql, aParameters);
        }

        public async Task<List<T>> SqlStoredProcedure<T>(string aSql, Dictionary<string, object> aParameters = null)
        {
            return await ExecuteSqlQuery<T>(CommandType.StoredProcedure, aSql, aParameters);
        }

        public async Task<T> ExecuteSqlQueryScalar<T>(string aSql, Dictionary<string, object> aParameters = null)
        {
            return await ExecuteSqlQueryScalar<T>(CommandType.Text, aSql, aParameters);
        }

        public async Task<T> ExecuteStoredProcedureScalar<T>(string aSql, Dictionary<string, object> aParameters = null)
        {
            return await ExecuteSqlQueryScalar<T>(CommandType.StoredProcedure, aSql, aParameters);
        }

        private async Task<List<Dictionary<string, object>>> ExecuteSqlQueryToDictionary(CommandType commandType, string aSql, Dictionary<string, object> aParameters)
        {
            List<Dictionary<string, object>> ll = new List<Dictionary<string, object>>();
            Dictionary<string, object> dic;
            DbConnection connection = Context.Database.GetDbConnection();
            DbCommand command = connection.CreateCommand();

            command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();
            command.CommandType = commandType;
            command.CommandText = aSql;

            SetParameters(command, aParameters);

            EnsureConnection(connection);

            DbDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                int i;
                object value;

                while (reader.Read())
                {
                    dic = new Dictionary<string, object>();

                    for (i = 0; i < reader.FieldCount; i++)
                    {
                        value = reader.GetValue(i);
                        dic.Add(reader.GetName(i), value == DBNull.Value ? null : value);
                    }

                    ll.Add(dic);
                }
            }

            reader.Close();

            return ll;
        }

        private async Task<List<T>> ExecuteSqlQuery<T>(CommandType commandType, string aSql, Dictionary<string, object> aParameters)
        {
            List<T> ll = new List<T>();
            DbConnection connection = Context.Database.GetDbConnection();
            DbCommand command = connection.CreateCommand();

            command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();
            command.CommandType = commandType;
            command.CommandText = aSql;

            SetParameters(command, aParameters);

            EnsureConnection(connection);

            DbDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                int i;
                string name;
                object value;
                bool boolObject = true;

                object item = Activator.CreateInstance<T>();
                Dictionary<string, PropertyInfo> dicProperties = item.GetType().GetProperties().ToDictionary(p => p.Name.ToLower());
                var dicTypes = dicProperties.Select(p =>
                {
                    Type underlyingTypeAux = Nullable.GetUnderlyingType(p.Value.PropertyType);

                    return new { p.Key, Type = underlyingTypeAux == null ? p.Value.PropertyType : underlyingTypeAux };
                }).ToDictionary(p => p.Key);

                Type underlyingType;

                while (reader.Read())
                {
                    item = Activator.CreateInstance<T>();

                    for (i = 0; i < reader.FieldCount; i++)
                    {
                        name = reader.GetName(i).ToLower();
                        value = reader.GetValue(i);

                        if (value != DBNull.Value && dicProperties.ContainsKey(name))
                        {
                            underlyingType = dicTypes[name].Type;

                            if (underlyingType.IsEnum)
                            {
                                dicProperties[name].SetValue(item, Enum.ToObject(underlyingType, value));
                            }
                            else if (underlyingType == boolObject.GetType())
                            {
                                dicProperties[name].SetValue(item, Convert.ToBoolean(value));
                            }
                            else
                            {
                                dicProperties[name].SetValue(item, value);
                            }
                        }
                    }

                    ll.Add((T)item);
                }
            }

            reader.Close();

            return ll;
        }

        private async Task<T> ExecuteSqlQueryScalar<T>(CommandType commandType, string aSql, Dictionary<string, object> aParameters)
        {
            DbConnection connection = Context.Database.GetDbConnection();
            DbCommand command = connection.CreateCommand();

            command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();
            command.CommandType = commandType;
            command.CommandText = aSql;

            SetParameters(command, aParameters);

            EnsureConnection(connection);

            return (T) (await command.ExecuteScalarAsync());
        }

        private void SetParameters(DbCommand command, Dictionary<string, object> aParameters)
        {
            if (aParameters != null)
            {
                foreach (var param in aParameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = param.Key;
                    p.Value = param.Value == null ? DBNull.Value : param.Value;

                    command.Parameters.Add(p);
                }
            }
        }

        private void EnsureConnection(DbConnection connection) {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
    }
}
