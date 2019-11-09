using MySql.Data.MySqlClient;
using System;

namespace NTDOY_Microservice.Models
{
    //data storage for Buy and Sell database operations
    public class BuySell
    {
        public string Username { get; set; }
        public string Account { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }

        //save a buy to the database and return the ID for final verification in central server
        public int LogBuy()
        {
            string query = "Insert into BuySell (b_type, username, t_account, price, quantity) " +
                           "Values (\"BUY\",\"" + Username + "\",\"" + Account + "\"," + Price + "," + Quantity + ")";
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
            string query = "Insert into BuySell (b_type, username, t_account, price, quantity) " +
                           "Values (\"SELL\",\"" + Username + "\",\"" + Account + "\"," + Price + "," + Quantity + ")";
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
