using MySql.Data.MySqlClient;
using System;

namespace NTDOY_Microservice.Models
{
    //model of database entry for a log of a single request to an api endpoint
    //gets logged by middleware after request finishes
    public class TransactionLog
    {
        public int id { get; set; }
        public string date_time { get; set; }
        public string type { get; set; }
        public float price { get; set; }
        public int quantity { get; set; }
        public string username { get; set; }

        //logs the information currently in this object to the database
        public void LogToDatabase()
        {
            string query = "Insert into transaction_logs (transaction_type, transaction_price, transaction_quantity, username) " +
                           "Values (\"" + type + "\"," + price + "," + quantity + ",\"" + username + "\")";
            try
            {
                MySqlCommand comm = DB_Connection.conn.CreateCommand();
                comm.CommandText = query;
                comm.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
