using System;
using MySql.Data.MySqlClient;
using System.Collections;

namespace NTDOY_Microservice.Models
{
    //model of database entry for a log of a single request to an api endpoint
    //gets logged by middleware after request finishes
    public class TransactionLog
    {
        public int id { get; set; }
        public string date_time { get; set; }
        public string type { get; set; }
        public string account { get; set; }
        public float price { get; set; }
        public int quantity { get; set; }
        public string username { get; set; }

        //logs the information currently in this object to the database
        public void LogToDatabase()
        {
            string query = "Insert into transaction_logs (t_type, t_account, t_price, t_quantity, username) " +
                           "Values (\"" + type + "\",\"" + account + "\"," + price + "," + quantity + ",\"" + username + "\")";
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

        //select all transaction logs from the database
        public static ArrayList GetAllTransactions()
        {
            ArrayList cols = new ArrayList();   //array list to store all columns

            string sql = "SELECT * FROM transaction_logs";
            MySqlCommand cmd = DB_Connection.conn.CreateCommand();
            cmd.CommandText = sql;
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                cols.Add(new
                {
                    t_id = rdr["t_id"],
                    t_time = rdr["t_time"],
                    t_type = rdr["t_type"],
                    t_account = rdr["t_account"],
                    t_price = rdr["t_price"],
                    t_quantity = rdr["t_quantity"],
                    username = rdr["username"]
                });
            }

            rdr.Close();
            return cols;

        }

        //gets a stock quoute json response string and gets the last price value from it
        public static string GetPriceFromJson(string json)
        {
            int indexOfLast = json.IndexOf("last");     //find index of variable we want to get from response
            int indexOfColon = json.IndexOf(':', indexOfLast);  //find start of the price
            return json.Substring(indexOfColon + 1, json.IndexOf(',', indexOfLast) - indexOfColon - 1); //substring out the price which lies between : and ,
        }
    }
}
