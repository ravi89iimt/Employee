using System.Data;
using System.Net;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeProject.Services.HelperService
{
    public class AdoHelper(IConfiguration configuration) : IAdoHelper
    {
        private readonly string _connectionString = configuration?.GetConnectionString("DefaultConnection");


        public async Task<int> ExecuteNonQueryAsync(string sql, SqlParameter[] parameters = null)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<DataTable> ExecuteQueryAsync(string sql, SqlParameter[] parameters = null)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteRecord(string tableName, string keyColumn, object keyValue)
        {
            try
            {
                string sql = $"DELETE FROM {tableName} WHERE {keyColumn} = @Key";
                SqlParameter param = new SqlParameter("@Key", keyValue);
                return await ExecuteNonQueryAsync(sql, new SqlParameter[] { param }) > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
    public interface IAdoHelper
    {
        Task<bool> DeleteRecord(string tableName, string keyColumn, object keyValue);
        Task<DataTable> ExecuteQueryAsync(string sql, SqlParameter[] parameters = null);
        Task<int> ExecuteNonQueryAsync(string sql, SqlParameter[] parameters = null);
    }

}
