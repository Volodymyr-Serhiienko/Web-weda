namespace WebApplication1
{
    public class AccessMiddleware
    {
        readonly RequestDelegate next;
        public AccessMiddleware(RequestDelegate next) => this.next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            if (path == "/Sys/Adm/Admin" && context.Session.GetString("email") != "utos.vovik@gmail.com")
                context.Response.Redirect("/Login");

            if (path == "/Index" || path == "/" || path == "/Contacts" || path == "/Login" || path == "/Register" || path == "/RegisterConfirm" || path == "/ForgotPassword" || path == "/PasswordConfirm" || path == "/Help")
            {
                await next.Invoke(context);
            }
            else
            {
                if (!context.Session.Keys.Contains("firstname"))
                    context.Response.Redirect("/Login");
                else
                    await next.Invoke(context);
            }
        }
    }
}