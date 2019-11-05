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
    public class BuyController : ControllerBase
    {
        [HttpPost]
        public async void BuyStock()
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
                    //headers not assigned properly so fail the buy operation
                    await Response.Body.WriteAsync(Encoding.ASCII.GetBytes("Could not complete buy operation."));
                    TransactionLog l = new TransactionLog();
                    l.type = "Failed Buy";
                    l.account = null;
                    l.price = 0f;
                    l.quantity = 0;
                    l.username = user.Username;
                    HttpContext.Items["Log"] = l;
                    return;
                }

                //TODO check if user has enough money for price * quantity in the listed account
                //find account current money and see if its less than cost of this buy
                //if it costs more than they have fail the buy else continue

                //log the purchase in the buy table
                BuySell buy = new BuySell();
                buy.username = user.Username;
                buy.account = account;
                buy.price = price;
                buy.quantity = quantity;
                int id = buy.LogBuy();

                //TODO subtract the purchase cost from their money and wait to see if successful
                //if it fails undo the buy at id and also fail the buy else continue to log

                //create transaction_log object that holds full info about this transaction
                await Response.Body.WriteAsync(Encoding.ASCII.GetBytes("Buy Successful"));
                TransactionLog log = new TransactionLog();
                log.type = "Buy";
                log.account = account;
                log.quantity = quantity;
                log.username = user.Username;
                log.price = price;
                HttpContext.Items["Log"] = log; //save log to be logged in middleware
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                await Response.Body.WriteAsync(Encoding.ASCII.GetBytes("Could not complete buy operation."));
                //create transaction_log object that holds full info about this transaction
                TransactionLog log = new TransactionLog();
                log.type = "Failed Buy";
                log.account = null;
                log.price = 0f;
                log.quantity = 0;
                log.username = ((User)HttpContext.Items["User"]).Username;
                HttpContext.Items["Log"] = log; //save log to be logged in middleware
            }
        }

    }
}