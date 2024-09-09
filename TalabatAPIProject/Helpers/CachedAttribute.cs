using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Text;
using System.Xml.Linq;
using Talabat.Core.Services;

namespace TalabatAPIProject.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSecond;

        public CachedAttribute(int timeToLiveInSecond) 
        {
            _timeToLiveInSecond = timeToLiveInSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          var responseCacheServer= context.HttpContext.RequestServices.GetRequiredService<IResponseCasheService>();
            //Aske CLR For Creating Object From ResponseCacheServer  Explicitly
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var response=await responseCacheServer.GetCashedResponseAsync(cacheKey);
            if(!string.IsNullOrEmpty(response) )
            {
                var result = new ContentResult()
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = result;
                return;
            } //Rsponse Is Not Cache
            var excutedActionContext= await next.Invoke(); //will excuted the next action Fillter Or The Action Itself
            if (excutedActionContext.Result is OkObjectResult objectResult && objectResult.Value is not null)
            {
                await responseCacheServer.CasheResponseAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSecond));
            }
         }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            // {{url}}/api/products?pageindex=1&pagesize=1&sort=name
            var Keybuilder = new StringBuilder();
            Keybuilder.Append(request.Path);//  /api/products

            //pageindex = 1
            //pagesize = 1
            //sort = name
            foreach (var (key,value) in request.Query)
            {
                Keybuilder.Append($"|{key}-{value}");
                //  /api/products|pageindex=1
                //  /api/products|pageindex=1 |pagesize = 1
                //  /api/products|pageindex-1 |pagesize-1 |sort-name

            }
            return Keybuilder.ToString();
        }
    }
}
