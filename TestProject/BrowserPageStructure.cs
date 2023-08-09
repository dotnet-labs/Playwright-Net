namespace TestProject
{
    [TestClass]
    public class BrowserPageStructure
    {
        [TestMethod]
        public async Task MultipleContexts()
        {
            using var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });

            await using var userContext = await browser.NewContextAsync();
            var userPage1 = await userContext.NewPageAsync();
            await userPage1.GotoAsync("https://playwright.dev/dotnet/");
            var userPage2 = await userContext.NewPageAsync();
            await userPage2.GotoAsync("https://playwright.dev/java/");

            await using var adminContext = await browser.NewContextAsync();
            var adminPage = await adminContext.NewPageAsync();
            await adminPage.GotoAsync("https://playwright.dev/");

            await userPage1.PauseAsync();
        }
    }
}
