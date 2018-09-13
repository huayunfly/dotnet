using System;
using System.Collections.Generic;
using System.Data;
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

        public static int ExecuteReader(string title)
        {
            int records = 0;
            var connection = new SqlConnection(DBConnectionSamples.GetConnectionString());
            var command = new SqlCommand(GetBookQuery(), connection);
            /* A more efficient method that’s more programming work is to use overloads of 
             * the Add method by passing the name and the SQL data type: 
             */
            var parameter = new SqlParameter("Title", SqlDbType.NVarChar, 40)
            {
                Value = $"{title}%"
            };
            command.Parameters.Add(parameter);

            connection.Open();
            using (SqlDataReader reader =
                    command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string bookTitle = reader.GetString(1);
                    string publisher = reader.GetString(2);
                    DateTime? releaseDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3);
                    records++;
                    Console.WriteLine($"{id, 5}. {bookTitle, -40}, {publisher, -15}, {releaseDate:d}");
                }

            }
            return records;
        }

        private static string GetBookQuery() => "SELECT [Id], [Title], [Publisher], [ReleaseDate] " +
                    "FROM [ProCSharp].[Books] WHERE lower([Title]) LIKE @Title " +
                    "ORDER BY [ReleaseDate] DESC";

    }
}
