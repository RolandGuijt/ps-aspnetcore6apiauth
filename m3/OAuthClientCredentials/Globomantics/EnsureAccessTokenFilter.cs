using Microsoft.AspNetCore.Mvc.Filters;

namespace Globomantics
{
    public class EnsureAccessTokenFilter : ActionFilterAttribute
    {
        private readonly HttpClient _HttpClient;

        public EnsureAccessTokenFilter(HttpClient httpClient)
        {
            _HttpClient = httpClient;
        }

        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            await _HttpClient.EnsureAccessTokenInHeader();
            await next();
        }
    }
}
