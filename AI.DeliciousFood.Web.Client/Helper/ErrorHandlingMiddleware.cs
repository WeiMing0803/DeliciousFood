namespace AI.DeliciousFood.Web.Client.Helper
{
    public class ErrorHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            await next(context);

            if (context.Response.StatusCode == StatusCodes.Status404NotFound && !context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.Redirect("/Home/NotFoundHtml");
            }
        }
    }
}
