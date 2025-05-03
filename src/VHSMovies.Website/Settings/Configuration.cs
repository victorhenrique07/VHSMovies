namespace VHSMovies.Website.Settings
{
    public static class Configuration
    {
        public static string HttpClientName = "vhsmovies";
        public static string BackendUrl => Environment.GetEnvironmentVariable("API_URL");
    }
}
