using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;

namespace com.huayunfly.app
{
    public class TransactionSamples
    {
        public static async Task TransactionSample()
        {
            using (var connection = new SqlConnection(DBConnectionSamples.GetConnectionString()))
            {
                await connection.OpenAsync();
                SqlTransaction tx = connection.BeginTransaction();

                try
                {
                    /* var command = new SqlCommand(GetTransctionString(), connection, trans); */
                    var command = new SqlCommand
                    {
                        CommandText = GetTransctionString(),
                        Connection = connection,
                        Transaction = tx
                    };
                    var p1 = new SqlParameter("Title", SqlDbType.NVarChar, 50);
                    var p2 = new SqlParameter("Publisher", SqlDbType.NVarChar, 50);
                    var p3 = new SqlParameter("Isbn", SqlDbType.NVarChar, 20);
                    var p4 = new SqlParameter("ReleaseDate", SqlDbType.Date);

                    command.Parameters.AddRange(new SqlParameter[] { p1, p2, p3, p4 });
                    command.Parameters["Title"].Value = "Professional C# 8 and .NET Core 3.0";
                    command.Parameters["Publisher"].Value = "Wrox Press";
                    command.Parameters["Isbn"].Value = "42-08154711";
                    command.Parameters["ReleaseDate"].Value = new DateTime(2020, 9, 2);

                    object id = await command.ExecuteScalarAsync();
                    Console.WriteLine($"record added with id: {id}");

                    command.Parameters["Title"].Value = "Professional C# 9 and .NET Core 3.0";
                    command.Parameters["Publisher"].Value = "Wrox Press";
                    /* Man made repeated Isbn to throw exception */
                    command.Parameters["Isbn"].Value = "42-08154711";
                    command.Parameters["ReleaseDate"].Value = new DateTime(2022, 9, 2);

                    id = await command.ExecuteScalarAsync();
                    Console.WriteLine($"record added with id: {id}");
                    tx.Commit();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error {ex.Message}, rolling back");
                    tx.Commit();
                }    
            }
        }

        private static string GetTransctionString() => 
            "INSERT INTO [ProCSharp].[Books] " + 
            "([Title], [Publisher], [Isbn], [ReleaseDate]) " +
            "VALUES(@Title, @Publisher, @Isbn, @ReleaseDate); " +
            "SELECT SCOPE_IDENTITY()";

    }
}
