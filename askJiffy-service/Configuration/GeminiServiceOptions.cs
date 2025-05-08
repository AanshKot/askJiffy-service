namespace askJiffy_service.Configuration
{
    public class GeminiServiceOptions
    {
        ///<summary>
        /// Setting Key for Json Settings (appsettings.json) binding (currently have nothing in there with key = "GeminiServiceOptions"
        /// will override in IServiceCollection AddGeminiService extension with setupAction
        ///</summary>
        public static readonly string SettingKey = "GeminiServiceOptions";

        public string GeminiDefaultBaseDomain { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public string Model { get; set; } = null!;
    }
}
