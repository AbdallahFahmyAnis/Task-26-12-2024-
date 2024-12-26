namespace TaskUI.Services
{
    public class ApiSettingsService
    {
        private readonly IConfiguration _configuration;

        public ApiSettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetApiBaseUrl()
        {
            return _configuration.GetSection("ApiSettings:BaseUrl").Value;
        }
    }

}
