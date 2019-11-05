using MySql.Data.MySqlClient;
using System;

namespace NTDOY_Microservice
{
    public class DB_Connection
    {
        public static MySqlConnection conn = null; //connection to db

        private static string connection_string = "server=35.224.129.168;uid=root;pwd=ntdoypassword;database=NTDOY_Logs";
        //attempt to connect to the database
        public static void Connect() {
            try
            {
                DB_Connection connection = new DB_Connection();
                conn = new MySqlConnection();
                conn.ConnectionString = connection_string;
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
                conn = null;
            }
        }
    }
}
