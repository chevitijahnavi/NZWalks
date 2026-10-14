using Azure.Core;
using System.Net;
using System.Reflection.Metadata;

namespace NZWalks.API.MiddleWares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // use of next() is it continues the request; catch handles an exception that occurs during that processing.
                await next(httpContext);
            }
            catch(Exception ex)
            {
                var errorId = Guid.NewGuid();

                //Log this Exception

                logger.LogError(ex,$"{errorId} :{ex.Message}");

                //Return a Custom Error Response

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong! we are looking into resolving this."
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }

        }
    }
}