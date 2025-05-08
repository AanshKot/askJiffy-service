using askJiffy_service.Configuration;

namespace askJiffy_service.Services.Extensions
{
    public static class GeminiServiceCollectionExtensions
    {
        //static extension method for IServiceCollection. It sets up a service (in this case, GeminiService) with options from configuration and allows custom overrides via an optional setupAction.
        public static IHttpClientBuilder AddGeminiService(this IServiceCollection services, Action<GeminiServiceOptions>? setupAction = null) 
        {
            //will try to bind section of appsettings.json i.e. "GeminiServiceOptions" (depending on the settingKey) to the GeminiServiceOptions class
            var optionsBuilder = services.AddOptions<GeminiServiceOptions>();
            optionsBuilder.BindConfiguration(GeminiServiceOptions.SettingKey);
            
            // this allows you to manually override or set properties after the config is bound. I used this to set the API key from the client secret
            if (setupAction != null)
            {
                optionsBuilder.Configure(setupAction);
            }

            //returns a typed HTTP client that has methods defined in IGeminiService that will be used to talk to Gemini's API
            // service is transient
            return services.AddHttpClient<IGeminiService, GeminiService>();
        }
    }
}
