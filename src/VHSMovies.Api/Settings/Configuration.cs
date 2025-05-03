namespace VHSMovies.Api.Settings
{
    public static class Configuration
    {
        public static string CorsPolicyName = "vhsmovies";
        public static string BackendUrl = Environment.GetEnvironmentVariable("API_URL");
        public static string FrontendUrl = Environment.GetEnvironmentVariable("WEBSITE_URL");
    }
}
