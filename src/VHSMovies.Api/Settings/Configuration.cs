namespace VHSMovies.Api.Settings
{
    public static class Configuration
    {
        public static string CorsPolicyName = "vhsmovies";
        private static string FrontendUrl { get; set; }

        public static string GetFrontendUrl(IWebHostEnvironment webHost)
        {
            FrontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL", EnvironmentVariableTarget.Process)
            ?? Environment.GetEnvironmentVariable("FRONTEND_URL", EnvironmentVariableTarget.User)
            ?? Environment.GetEnvironmentVariable("FRONTEND_URL", EnvironmentVariableTarget.Machine)
            ?? Environment.GetEnvironmentVariable("FRONTEND_URL");

            if (string.IsNullOrWhiteSpace(FrontendUrl))
                throw new InvalidOperationException("The front-end url was not defined.");

            return FrontendUrl;
        }
    }
}
