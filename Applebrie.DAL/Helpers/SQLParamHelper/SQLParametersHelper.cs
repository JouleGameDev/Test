using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Applebrie.DAL.Helpers.SQLParamHelper
{
    public static class SQLParametersHelper
    {
        public static object[] ToSqlParamsArray(this object obj, SqlParameter[] additionalParams = null)
        {
            var result = ToSqlParamsList(obj, additionalParams);
            return result.ToArray<object>();
        }

        private static IEnumerable<SqlParameter> ToSqlParamsList(this object obj,
            SqlParameter[] additionalParams = null)
        {
            var props = (
                from p in obj.GetType().GetProperties()
                let nameAttr = p.GetCustomAttributes(typeof(QueryParamNameAttribute), true)
                let ignoreAttr = p.GetCustomAttributes(typeof(QueryParamIgnoreAttribute), true)
                select new { Property = p, Names = nameAttr, Ignores = ignoreAttr }).ToList();

            var result = new List<SqlParameter>();

            props.ForEach(p =>
            {
                if (p.Ignores != null && p.Ignores.Length > 0)
                    return;

                var name = p.Names.FirstOrDefault() as QueryParamNameAttribute;
                var pinfo = new QueryParamInfo();

                if (name != null && !string.IsNullOrWhiteSpace(name.Name))
                    pinfo.Name = name.Name.Replace("@", "");
                else
                    pinfo.Name = p.Property.Name.Replace("@", "");

                pinfo.Value = p.Property.GetValue(obj) ?? DBNull.Value;
                if (p.Property.PropertyType.IsEnum && pinfo.Value != DBNull.Value)
                    pinfo.Value = (int)pinfo.Value;
                SqlParameter sqlParam = null;
                if (p.Property.PropertyType == typeof(string))
                    sqlParam = new SqlParameter(pinfo.Name, TypeConvertor.ToSqlDbType(p.Property.PropertyType), -1)
                    {
                        Value = pinfo.Value
                    };
                else if (p.Property.PropertyType.Name.Contains("IEnumerable`1"))
                {
                    sqlParam = new SqlParameter(pinfo.Name, SqlDbType.Structured)
                    {
                        Value = pinfo.Value
                    };
                }
                else
                    sqlParam = new SqlParameter(pinfo.Name, TypeConvertor.ToSqlDbType(p.Property.PropertyType))
                    {
                        Value = pinfo.Value
                    };


                result.Add(sqlParam);
            });

            if (additionalParams != null && additionalParams.Length > 0)
                result.AddRange(additionalParams);

            return result;
        }

        private class QueryParamInfo
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }


        [AttributeUsage(AttributeTargets.Property)]
        public class QueryParamNameAttribute : Attribute
        {
            public QueryParamNameAttribute(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }

        /// <summary>
        ///     Ignore this property
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class QueryParamIgnoreAttribute : Attribute
        {
        }

        #region TypeConventor
        public static class TypeConvertor
        {
            private static readonly ArrayList _DbTypeList = new ArrayList();

            #region Constructors

            static TypeConvertor()
            {
                var dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(int), DbType.Int32, SqlDbType.Int);
                _DbTypeList.Add(dbTypeMapEntry);
                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(bool), DbType.Boolean, SqlDbType.Bit);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(byte), DbType.Double, SqlDbType.TinyInt);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(byte[]), DbType.Binary, SqlDbType.Image);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(decimal), DbType.Decimal, SqlDbType.Decimal);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(double), DbType.Double, SqlDbType.Float);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(short), DbType.Int16, SqlDbType.SmallInt);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(long), DbType.Int64, SqlDbType.BigInt);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(object), DbType.Object, SqlDbType.Variant);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.VarChar);
                _DbTypeList.Add(dbTypeMapEntry);

                dbTypeMapEntry
                    = new DbTypeMapEntry(typeof(float), DbType.Single, SqlDbType.Float);
                _DbTypeList.Add(dbTypeMapEntry);
            }

            #endregion

            private readonly struct DbTypeMapEntry
            {
                public readonly Type Type;
                public readonly DbType DbType;
                public readonly SqlDbType SqlDbType;

                public DbTypeMapEntry(Type type, DbType dbType, SqlDbType sqlDbType)
                {
                    Type = type;
                    DbType = dbType;
                    SqlDbType = sqlDbType;
                }
            }

            #region Methods

            /// <summary>
            ///     Convert db type to .Net data type
            /// </summary>
            /// <param name="dbType"></param>
            /// <returns></returns>
            public static Type ToNetType(DbType dbType)
            {
                var entry = Find(dbType);
                return entry.Type;
            }

            /// <summary>
            ///     Convert T-SQL type to .Net data type
            /// </summary>
            /// <param name="sqlDbType"></param>
            /// <returns></returns>
            public static Type ToNetType(SqlDbType sqlDbType)
            {
                var entry = Find(sqlDbType);
                return entry.Type;
            }

            /// <summary>
            ///     Convert .Net type to Db type
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static DbType ToDbType(Type type)
            {
                var entry = Find(type);
                return entry.DbType;
            }

            /// <summary>
            ///     Convert T-SQL data type to DbType
            /// </summary>
            /// <param name="sqlDbType"></param>
            /// <returns></returns>
            public static DbType ToDbType(SqlDbType sqlDbType)
            {
                var entry = Find(sqlDbType);
                return entry.DbType;
            }

            /// <summary>
            ///     Convert .Net type to T-SQL data type
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static SqlDbType ToSqlDbType(Type type)
            {
                var entry = Find(type);
                return entry.SqlDbType;
            }

            /// <summary>
            ///     Convert DbType type to T-SQL data type
            /// </summary>
            /// <param name="dbType"></param>
            /// <returns></returns>
            public static SqlDbType ToSqlDbType(DbType dbType)
            {
                var entry = Find(dbType);
                return entry.SqlDbType;
            }

            private static DbTypeMapEntry Find(Type type)
            {
                object retObj = null;
                foreach (var t in _DbTypeList)
                {
                    var entry = (DbTypeMapEntry)t;

                    if (entry.Type != (Nullable.GetUnderlyingType(type) ?? type)) continue;

                    retObj = entry;
                    break;
                }

                if (retObj == null && type.IsEnum)
                    retObj = _DbTypeList[0];

                if (retObj == null)
                    throw
                        new ApplicationException("Referenced an unsupported Type " + type);

                return (DbTypeMapEntry)retObj;
            }

            private static DbTypeMapEntry Find(DbType dbType)
            {
                object retObj = null;
                foreach (var type in _DbTypeList)
                {
                    var entry = (DbTypeMapEntry)type;

                    if (entry.DbType != dbType) continue;

                    retObj = entry;
                    break;
                }

                if (retObj == null)
                    throw
                        new ApplicationException("Referenced an unsupported DbType " + dbType);

                return (DbTypeMapEntry)retObj;
            }

            private static DbTypeMapEntry Find(SqlDbType sqlDbType)
            {
                object retObj = null;
                foreach (var type in _DbTypeList)
                {
                    var entry = (DbTypeMapEntry)type;

                    if (entry.SqlDbType != sqlDbType) continue;

                    retObj = entry;
                    break;
                }

                if (retObj == null)
                    throw
                        new ApplicationException("Referenced an unsupported SqlDbType");

                return (DbTypeMapEntry)retObj;
            }
            #endregion
        }
        #endregion
    }
}
