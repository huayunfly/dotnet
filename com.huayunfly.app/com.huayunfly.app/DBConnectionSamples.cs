using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace com.huayunfly.app
{
    public class DBConnectionSamples
    {
        public static bool OpenConnection()
        {
            string connectionString = @"server=(localdb)\MSSQLLocalDB;" +
                "integrated security=SSPI; database=Books";
            return OpenConnection(connectionString);
        }

        public static bool OpenConnectionUsingConfig()
        {
            /* SetBasePath() is in Microsoft.Extensions.Configuration.Json */
            var configurationBuilder = 
                new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("db_connection_config.json");
            IConfiguration config = configurationBuilder.Build();
            string connectionString = config["Data:DefaultConnection:ConnectionString"];
            return OpenConnection(connectionString);

        }

        private static bool OpenConnection(string connectionString)
        {
            bool success = false;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                success = true;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Database connection failed: {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return success;
        }

    }
}
