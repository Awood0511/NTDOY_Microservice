using MySql.Data.MySqlClient;
using System;

namespace NTDOY_Microservice
{
    public class DB_Connection
    {
        public static MySqlConnection conn = null; //connection to be used by every function thats needs DB

        private static string connection_string = "server=35.224.129.168;uid=root;pwd=ntdoypassword;database=NTDOY_Logs";
        //attempt to connect to the database
        public static void Connect() {
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = connection_string;
                conn.Open();
                Console.WriteLine("Connected to DB");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
                conn = null;
            }
        }
    }
}
