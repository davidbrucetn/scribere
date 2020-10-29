using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;


namespace Scribere.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class APIKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeader = "ApiKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeader, out var potentialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var configuration = context.HttpContext.RequestServices.GetRequiredService <IConfiguration>();

            var apiKey = configuration.GetValue <string> (key: "GoogleAPIKey");//To read API key from appsettings.json use this line
            //var apiKey = "MyAPIKey123";  //To hard code the API key within your code, use this line
            if (!apiKey.Equals(potentialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }

}
