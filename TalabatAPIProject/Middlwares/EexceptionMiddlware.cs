using System.Net;
using System.Text.Json;
using TalabatAPIProject.Error;

namespace TalabatAPIProject.Middlwares
{
    // By Convension
    public class EexceptionMiddlware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<EexceptionMiddlware> logger;
        private readonly IHostEnvironment environment;

        public EexceptionMiddlware(RequestDelegate Next, ILogger<EexceptionMiddlware>logger,IHostEnvironment environment)
        {
            next = Next;
            this.logger = logger;
            this.environment = environment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
             await  next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var Response = environment.IsDevelopment() ?
                    new ApiExceeptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new ApiExceeptionResponse((int)HttpStatusCode.InternalServerError);
                // المشكله ان هو هنا بسكال كيس وهو بيتعامل ك كمل كيس 
                 var option =new JsonSerializerOptions(){ PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
                var json=JsonSerializer.Serialize(Response,option);
                 await httpContext.Response.WriteAsync(json);
                
            }
        }

    }
}
