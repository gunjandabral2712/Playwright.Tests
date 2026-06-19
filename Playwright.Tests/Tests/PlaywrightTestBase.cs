using Microsoft.Playwright;
using Playwright.Framework.TestInfrastructure;

namespace Playwright.Tests.Tests
{
    public class PlaywrightTestBase
    {
        protected IPlaywright? Playwright;
        protected IBrowser? Browser;
        protected IBrowserContext? Context;
        protected IPage? Page;
        protected TestConfig? Config;

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            Config = TestConfig.Load();

            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            var headless = Config.Headless;
            var browserChoice = (Config.Browser ?? "chromium").ToLowerInvariant();

            if (browserChoice == "chromium")
                Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless });
            else if (browserChoice == "firefox")
                Browser = await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless });
            else if (browserChoice == "webkit")
                Browser = await Playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless });
            else
                throw new InvalidOperationException($"Unsupported browser: {Config.Browser}");
        }

        [SetUp]
        public async Task Setup()
        {
            var browser = Browser ?? throw new InvalidOperationException("Browser has not been initialized. Ensure GlobalSetup executed successfully.");
            var context = await browser.NewContextAsync();
            Context = context;
            Page = await context.NewPageAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            var context = Context;
            if (context != null)
                await context.CloseAsync();
        }

        [OneTimeTearDown]
        public async Task GlobalTeardown()
        {
            var browser = Browser;
            if (browser != null)
                await browser.CloseAsync();
            Playwright?.Dispose();
        }
    }
}
