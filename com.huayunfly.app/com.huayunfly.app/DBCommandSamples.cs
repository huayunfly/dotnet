using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace com.huayunfly.app
{
    public class DBCommandSamples
    {
        public static int ExecuteNoQuery()
        {
            int records = -1;
            try
            {
                using (var connection = new SqlConnection(DBConnectionSamples.GetConnectionString()))
                {
                    string sql = "DELETE FROM [ProCSharp].[Books] " +
                        "WHERE [Isbn] = @Isbn AND [Title] = @Title";
                    var command = new SqlCommand(sql, connection);
                    /** Don't use string concatenation with SQL parameters,
                     * this is often misused in SQL injection attack.
                     */
                    command.Parameters.AddWithValue("Title", "Introduction to Programming in Java, 2nd Edition");
                    command.Parameters.AddWithValue("Publisher", "Addison Wesley");
                    command.Parameters.AddWithValue("Isbn", "456-1119449270");
                    command.Parameters.AddWithValue("ReleaseDate", new DateTime(2017, 4, 1));

                    connection.Open();
                    /* Delete the existed one */
                    command.ExecuteNonQuery();

                    sql = "INSERT INTO [ProCSharp].[Books] " + 
                        "([Title], [Publisher], [Isbn], [ReleaseDate]) " + 
                        "VALUES (@Title, @Publisher, @Isbn, @ReleaseDate)";
                    command.CommandText = sql;
                    records = command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            return records;
        }

    }
}
