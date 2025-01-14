namespace EXONLOG
{
    using Microsoft.Data.SqlClient;
    using System.Threading.Tasks;

    public static class ConnectionHelper
    {
        public static async Task<bool> TestConnectionAsync(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class ConnectionStringService
    {
        public string ActiveConnectionString { get; set; }
    }
}
