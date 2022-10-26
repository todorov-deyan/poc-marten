namespace PocMarten.Api.ExceptionHandler
{
    public static class UseGlobalExceptionExtension
    {   
        public static void UseGlobalException(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
