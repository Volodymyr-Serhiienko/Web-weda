using WebApplication1.Services;

namespace WebApplication1
{
    public static class ServiceProviderExtensions
    {
        public static void AddMyServices(this IServiceCollection services)
        {
            services.AddSingleton<BukvitsaService>();
            services.AddSingleton<EmailService>();
            services.AddScoped(typeof(ICaptchaValidator), typeof(ReCaptchaValidator));
            services.AddHttpClient();
			services.AddTransient<ILogWriterService, LogWriterService>(serviceProvider => 
            {
				var textWriter = new StreamWriter("log.txt", true);
				return new LogWriterService(textWriter);
			});
		}
    }
}
