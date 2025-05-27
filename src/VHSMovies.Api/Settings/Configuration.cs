namespace VHSMovies.Api.Settings
{
    public static class Configuration
    {
        public static string CorsPolicyName = "vhsmovies";
        private static string FrontendUrl { get; set; }

        public static string GetFrontendUrl(IWebHostEnvironment webHost)
        {
            FrontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL", webHost.IsDevelopment() ? EnvironmentVariableTarget.Machine : EnvironmentVariableTarget.Process);

            return FrontendUrl;
        }
    }
}
