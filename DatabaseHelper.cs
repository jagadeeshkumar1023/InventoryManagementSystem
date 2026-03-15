using System.Data.SqlClient;

namespace InventoryManagementSystem.DataAccess
{
    public class DatabaseHelper
    {
        private static string connectionString =
        "Data Source=.;Initial Catalog=InventoryDB;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static int ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}