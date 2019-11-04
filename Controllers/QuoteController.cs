using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using System.IO;
using System.Text;
using NTDOY_Microservice.Models;

namespace NTDOY_MicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        //gets NTDOY stock information from tradier
        [HttpGet]
        public async void GetStockQuote()
        {
            //make request to tradier
            try
            {
                //send tradier response to user
                var response = (HttpWebResponse)HttpContext.Items["WebResponse"];
                var user = (User)HttpContext.Items["User"];

                var memoryStream = new MemoryStream();
                response.GetResponseStream().CopyTo(memoryStream);
                Response.ContentType = "application/json";
                await Response.Body.WriteAsync(memoryStream.ToArray());

                //create transaction_log object that holds full info about this transaction
                TransactionLog log = new TransactionLog();
                log.type = "Quote";
                log.price = 0f;
                log.quantity = 0;
                log.username = user.Username;
                HttpContext.Items["Log"] = log; //save log to be logged in middleware

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                await Response.Body.WriteAsync(Encoding.ASCII.GetBytes("Could not get stock information from Tradier."));
                //create transaction_log object that holds full info about this transaction
                TransactionLog log = new TransactionLog();
                log.type = "Failed Stock Lookup";
                log.price = 0f;
                log.quantity = 0;
                log.username = ((User)HttpContext.Items["User"]).Username;
                HttpContext.Items["Log"] = log; //save log to be logged in middleware
            }
        }

    }
}