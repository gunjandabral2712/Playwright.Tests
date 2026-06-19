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

            // Start tracing for this test so we can collect trace on failure
            try
            {
                await context.Tracing.StartAsync(new TracingStartOptions { Screenshots = true, Snapshots = true, Sources = true });
            }
            catch
            {
                // tracing may not be supported in some environments; ignore start failures
            }
        }

        [TearDown]
        public async Task TearDown()
        {
            var context = Context;

            // Ensure artifacts directory in project root exists (Playwright.Tests/artifacts)
            string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var artifactsDir = Path.Combine(projectDir, "artifacts");
            Directory.CreateDirectory(artifactsDir);

            // If test failed, capture screenshot and save trace
            var outcome = NUnit.Framework.TestContext.CurrentContext.Result.Outcome.Status;
            var testName = NUnit.Framework.TestContext.CurrentContext.Test.Name ?? "test";

            if (context != null)
            {
                try
                {
                    if (outcome == NUnit.Framework.Interfaces.TestStatus.Failed && Page != null)
                    {
                        var screenshotPath = Path.Combine(artifactsDir, testName + ".png");
                        try
                        {
                            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
                        }
                        catch
                        {
                            // ignore screenshot errors
                        }
                    }

                    // Stop tracing and save to artifacts regardless of test outcome
                    var tracePath = Path.Combine(artifactsDir, testName + "-trace.zip");
                    try
                    {
                        await context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
                    }
                    catch
                    {
                        // ignore tracing stop errors
                    }
                }
                catch
                {
                    // ignore any teardown exceptions
                }

                try
                {
                    await context.CloseAsync();
                }
                catch
                {
                    // ignore close errors
                }
            }
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
