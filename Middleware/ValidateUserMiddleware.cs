﻿using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Primitives;
using NTDOY_MicroService.Models;
using Microsoft.AspNetCore.Builder;
using NTDOY_Microservice.Models;

namespace NTDOY_MicroService.Middleware
{
    public class ValidateUserMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidateUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //attempts to validate the user making the request
        //passes user info as request headers if successful
        //returns access denied if no token or token fails
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers.TryGetValue("Authorization", out StringValues values);
            String token;

            if (values.Count == 1)
            {
                token = values[0];
            }
            else
            {
                context.Response.StatusCode = 403;
                await context.Response.Body.WriteAsync(Encoding.ASCII.GetBytes("No Access Token Provided"));

                //create transaction_log object that holds full info about this transaction
                TransactionLog log = new TransactionLog
                {
                    Type = "Failed Authorization"
                };
                context.Items["Log"] = log; //save log to be logged in middleware
                return;
            }


            try
            {
                User user = TokenManager.ValidateToken(token);
                if (user == null)
                {
                    context.Response.StatusCode = 403;
                    await context.Response.Body.WriteAsync(Encoding.ASCII.GetBytes("Invalid Token"));
                    //create transaction_log object that holds full info about this transaction
                    TransactionLog log = new TransactionLog
                    {
                        Type = "Failed Authorization"
                    };
                    context.Items["Log"] = log; //save log to be logged in middleware
                }
                else
                {
                    context.Items["User"] = user;   //attach user object to Items

                    await _next(context);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                context.Response.StatusCode = 403;
                await context.Response.Body.WriteAsync(Encoding.ASCII.GetBytes("Invalid Token"));
                //create transaction_log object that holds full info about this transaction
                TransactionLog log = new TransactionLog
                {
                    Type = "Failed Authorization"
                };
                context.Items["Log"] = log; //save log to be logged in middleware
            }
        }
    }

    public static class ValidateUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidateUserMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidateUserMiddleware>();
        }
    }
}
