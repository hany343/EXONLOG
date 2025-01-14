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
                // Create a connection string builder to parse and modify the connection string
                var builder = new SqlConnectionStringBuilder(connectionString)
                {
                    InitialCatalog = "" // Remove the database name to test connection to the server
                };

                using var connection = new SqlConnection(builder.ConnectionString);
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
