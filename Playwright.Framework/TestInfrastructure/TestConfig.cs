using Microsoft.Extensions.Configuration;

namespace Playwright.Framework.TestInfrastructure
{
    public class TestConfig
    {
        public string? Browser { get; set; }
        public bool Headless { get; set; }
        public string? BaseUrl { get; set; }

        public static TestConfig Load()
        {
            // Use the test assembly base directory so the appsettings.json file copied to output is found
            var basePath = AppContext.BaseDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var config = new TestConfig();
            configuration.Bind(config);
            return config;
        }
    }
}
