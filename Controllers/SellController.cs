using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NTDOY_Microservice.Models;
using NTDOY_MicroService;

namespace NTDOY_Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellController : ControllerBase
    {
        [HttpPost]
        public async void SellStock()
        {
            try
            {
                //get the headers
                StringValues quan, acc; //headers for qunatity and account name
                string account;
                int quantity;
                Request.Headers.TryGetValue("quantity", out quan);
                Request.Headers.TryGetValue("account", out acc);

                //parse out the price
                var response = (HttpWebResponse)HttpContext.Items["WebResponse"];
                var user = (User)HttpContext.Items["User"];
                var memoryStream = new MemoryStream();
                response.GetResponseStream().CopyTo(memoryStream);
                string json = Encoding.ASCII.GetString(memoryStream.ToArray());  //turn json into string to be readable
                float price = float.Parse(TransactionLog.GetPriceFromJson(json));

                if (quan.Count == 1 && acc.Count == 1)
                {
                    quantity = Int32.Parse(quan[0]);
                    account = acc[0];
                }
                else
                {
                    //headers not assigned properly so fail the sell operation
                    await Response.Body.WriteAsync(Encoding.ASCII.GetBytes("Could not complete sell operation."));
                    TransactionLog l = new TransactionLog();
                    l.type = "Failed Sell";
                    l.account = null;
                    l.price = 0f;
                    l.quantity = 0;
                    l.username = user.Username;
                    HttpContext.Items["Log"] = l;
                    return;
                }

                //TODO check if user has enough stock in this account to sell the listed quantity
                //find account current stock in this account and see if its greater or equal to quantity
                //fail if it is less than

                //log the sale in the sell table
                BuySell sell = new BuySell();
                sell.username = user.Username;
                sell.account = account;
                sell.price = price;
                sell.quantity = quantity;
                int id = sell.LogSell();

                //TODO add the sell cost to the users account
                //if it fails undo the sell at id and also fail the sell else continue to log

                //create transaction_log object that holds full info about this transaction
                await Response.Body.WriteAsync(Encoding.ASCII.GetBytes("Sell Successful"));
                TransactionLog log = new TransactionLog();
                log.type = "Sell";
                log.account = account;
                log.quantity = quantity;
                log.username = user.Username;
                log.price = price;
                HttpContext.Items["Log"] = log; //save log to be logged in middleware
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                await Response.Body.WriteAsync(Encoding.ASCII.GetBytes("Could not complete Sell operation."));
                //create transaction_log object that holds full info about this transaction
                TransactionLog log = new TransactionLog();
                log.type = "Failed Sell";
                log.account = null;
                log.price = 0f;
                log.quantity = 0;
                log.username = ((User)HttpContext.Items["User"]).Username;
                HttpContext.Items["Log"] = log; //save log to be logged in middleware
            }
        }

    }
}