using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTDOY_Microservice.Models
{
    //data storage for Buy and Sell database operations
    public class BuySell
    {
        public string username { get; set; }
        public string account { get; set; }
        public float price { get; set; }
        public int quantity { get; set; }

        //save a buy to the database and return the ID for final verification in central server
        public int LogBuy()
        {
            string query = "Insert into buy (username, t_account, price, quantity) " +
                           "Values (\"" + username + "\",\"" + account + "\"," + price + "," + quantity + ")";
            try
            {
                MySqlCommand comm = DB_Connection.conn.CreateCommand();
                comm.CommandText = query;
                comm.ExecuteNonQuery();
                return (int)comm.LastInsertedId;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }

        //save a sell to the database and return the ID for final verification in central server
        public int LogSell()
        {
            string query = "Insert into sell (username, t_account, price, quantity) " +
                           "Values (\"" + username + "\",\"" + account + "\"," + price + "," + quantity + ")";
            try
            {
                MySqlCommand comm = DB_Connection.conn.CreateCommand();
                comm.CommandText = query;
                comm.ExecuteNonQuery();
                return (int)comm.LastInsertedId;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
    }

}
