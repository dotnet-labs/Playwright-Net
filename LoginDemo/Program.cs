using Microsoft.Playwright;


using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    SlowMo = 50,
});

var page = await browser.NewPageAsync(new BrowserNewPageOptions
{
    RecordVideoDir = "videos/"
});
// Go to http://the-internet.herokuapp.com/login
await page.GotoAsync("http://the-internet.herokuapp.com/login");
// Fill input[name="username"]
await page.FillAsync("input[name=\"username\"]", "tomsmith");
// Fill input[name="password"]
await page.FillAsync("input[name=\"password\"]", "SuperSecretPassword!");
// Click button:has-text("Login")
await page.ClickAsync("button:has-text(\"Login\")");

await page.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshot.png" });
