using Applebrie.DAL.Interfaces;
using Applebrie.DAL.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Applebrie.DAL.Helpers.SQLParamHelper;

namespace Applebrie.DAL.Repositories
{
    public class SQLRepository : IRepository
    {
        private readonly SQLDBContextOptions _options;

        protected SQLRepository(IOptions<SQLDBContextOptions> options)
        {
            _options = options.Value;
        }

        public async Task<SqlConnection> GetConnection()
        {
            try
            {
                var con = new SqlConnection(_options.DbConnectionString);
                return con;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> ExecuteJsonQueryAsync(string query, object sqlParams = null,
            CommandType commandType = CommandType.Text)
        {
            try
            {
                var res = new StringBuilder(500);

                await using var cnn = await GetConnection();
                await using var cmd = new SqlCommand(query, cnn) { CommandType = commandType };


                if (sqlParams != null)
                {
                    if (sqlParams is List<SqlParameter> list)
                        cmd.Parameters.AddRange(list.ToArray());
                    else
                        cmd.Parameters.AddRange(sqlParams.ToSqlParamsArray());
                }

                await cnn.OpenAsync();

                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read()) res.Append(reader.GetString(0));

                return res.ToString();  
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<T> ExecuteJsonResultProcedureAsync<T>(string query, object sqlParams = null)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(await ExecuteJsonQueryAsync(query, sqlParams,
                    CommandType.StoredProcedure));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
