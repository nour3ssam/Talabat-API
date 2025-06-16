using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.ServiceInterfaces;

namespace TalabatAPI.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int expireTimeInSecond;

        public CachedAttribute(int ExpireTimeInSecond)
        {
            expireTimeInSecond = ExpireTimeInSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var CacheKey = GenerateCacheKeyFormRequest(context.HttpContext.Request);
            var CachedResponse= await CacheService.GetCacheDataAsync(CacheKey);
            if (CachedResponse != null) {
                var ContentResult = new ContentResult()
                {
                    Content= CachedResponse,
                    ContentType="application/json",
                    StatusCode=200

                };
                context.Result = ContentResult;
                return;
            }
            var ExecutedEndPointContext= await next.Invoke(); // execute EndPoint
            if(ExecutedEndPointContext.Result is OkObjectResult result)
            {
               await CacheService.CacheResponceAsync(CacheKey, result.Value, TimeSpan.FromSeconds(expireTimeInSecond));
            }

        }

        private string GenerateCacheKeyFormRequest(HttpRequest Request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(Request.Path);// Api/Product
            foreach (var (Key, Value) in Request.Query.OrderBy(o => o.Key))
            {
                // Sort = Name
                //PageSize =1
                //PageIndex =5

                KeyBuilder.Append($"|{Key}-{Value}");
                // Api/Product|PageIndex-5|PageSize-1|Sort-Name
            }
            return KeyBuilder.ToString();
        }
    }
}
