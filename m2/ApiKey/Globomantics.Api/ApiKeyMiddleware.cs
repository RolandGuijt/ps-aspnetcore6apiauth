namespace Globomantics.Api
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _ApiKeyName = "XApiKey";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IConfiguration config)
        {
            var apiKeyPresentInHeader = context.Request.Headers.TryGetValue(_ApiKeyName, out var extractedApiKey);
            var apiKey = config[_ApiKeyName];

            if ((apiKeyPresentInHeader && apiKey == extractedApiKey)
                || context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid Api Key");
        }
    }

    public static class ApiKeyMiddlewareExtension
    {
        public static IApplicationBuilder UseApiKey(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
