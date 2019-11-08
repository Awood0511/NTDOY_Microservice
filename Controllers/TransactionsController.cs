using System;
using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NTDOY_Microservice.Models;
using NTDOY_MicroService;

namespace NTDOY_Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        //Gets all logs from the db and returns as JSON
        //only if the authenticated user is the Admin
        [HttpGet]
        public async void GetTransactions()
        {
            User user = (User)HttpContext.Items["User"];

            try
            {
                TransactionLog log = new TransactionLog();

                if(user.Username == "admin")
                {
                    //get transactions from database
                    ArrayList trans = TransactionLog.GetAllTransactions();
                    HttpContext.Response.ContentType = "application/json";
                    await HttpContext.Response.Body.WriteAsync(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(trans)));
                    log.type = "Admin Viewed Logs";
                }
                else
                {
                    HttpContext.Response.StatusCode = 400;
                    await HttpContext.Response.Body.WriteAsync(Encoding.ASCII.GetBytes("You do not have permission."));
                    log.type = "Non Admin Blocked from Logs";
                }

                log.username = user.Username;
                log.account = "";
                log.price = 0f;
                log.quantity = 0;
                HttpContext.Items["Log"] = log;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                User u = (User)HttpContext.Items["User"];
                TransactionLog log = new TransactionLog();
                log.type = "Error During Log View";
                log.username = user.Username;
                log.account = "";
                log.price = 0f;
                log.quantity = 0;
                HttpContext.Items["Log"] = log;
                HttpContext.Response.StatusCode = 404;
                await HttpContext.Response.Body.WriteAsync(Encoding.ASCII.GetBytes("Something went wrong."));
            }
        }
    }
}