namespace TestProject;

[TestClass]
public class FormTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            RecordVideoDir = "videos",
            RecordVideoSize = new RecordVideoSize { Width = 1920, Height = 1080 },
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        };
    }

    // dotnet test --filter "Name~FillInForm" -- Playwright.LaunchOptions.SlowMo=100
    // dotnet test --filter "Name~FillInForm" -- Playwright.LaunchOptions.Headless=false Playwright.LaunchOptions.SlowMo=200
    // dotnet test --filter "Name~FillInForm" --settings:test.runsettings.xml
    [TestMethod]
    public async Task FillInForm()
    {
        await Page.GotoAsync("https://changhuixu.github.io/uiowa-header-demo/form-wizards");

        await Page.Locator("date-range-picker").GetByRole(AriaRole.Button).ClickAsync();
        await Page.GetByLabel("Select month").SelectOptionAsync(new[] { "8" });
        await Page.GetByLabel("Select year").SelectOptionAsync(new[] { "2023" });
        await Page.GetByText("31").Nth(2).ClickAsync();
        await Page.GetByText("6", new() { Exact = true }).Nth(1).ClickAsync();

        await Page.GetByLabel("Room Type").SelectOptionAsync(new[] { "0: Object" });

        await Page.GetByText("Breakfast").ClickAsync();
        await Page.GetByText("WiFi").ClickAsync();
        await Page.GetByText("Parking Lot").ClickAsync();

        await Page.GetByPlaceholder("HH").ClickAsync();
        await Page.GetByPlaceholder("HH").FillAsync("3");
        await Page.GetByPlaceholder("MM").ClickAsync();
        await Page.GetByPlaceholder("MM").FillAsync("30");
        await Page.GetByRole(AriaRole.Button, new() { Name = "AM" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();

        await Page.GetByPlaceholder("First Name").ClickAsync();
        await Page.GetByPlaceholder("First Name").FillAsync("My");
        await Page.GetByPlaceholder("First Name").PressAsync("Tab");
        await Page.GetByPlaceholder("MI").PressAsync("Tab");
        await Page.GetByPlaceholder("Last Name").FillAsync("Name");
        await Page.GetByPlaceholder("Last Name").PressAsync("Tab");

        await Page.GetByPlaceholder("Email").FillAsync("my-name@company.com");
        await Page.GetByPlaceholder("Email").PressAsync("Tab");

        await Page.GetByPlaceholder("(319) 123-1234").FillAsync("3190001234");
        await Page.GetByPlaceholder("(319) 123-1234").PressAsync("Tab");

        await Page.GetByPlaceholder("1234 Main St").FillAsync("1234 Main St");
        await Page.GetByPlaceholder("1234 Main St").PressAsync("Tab");

        await Page.GetByPlaceholder("Apartment, studio, or floor").PressAsync("Tab");

        await Page.GetByLabel("City").FillAsync("Iowa City");
        await Page.GetByLabel("City").PressAsync("Tab");
        await Page.GetByLabel("State").SelectOptionAsync(new[] { "IA" });
        await Page.GetByLabel("State").PressAsync("Tab");
        await Page.GetByLabel("Zip").FillAsync("52240");

        await Page.GetByText("Remember me").ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirm" }).ClickAsync();
    }

    [TestMethod]
    public async Task FormTestTracing()
    {
        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        await FillInForm();

        await Context.Tracing.StopAsync(new()
        {
            Path = "trace.zip"
        });
    }
}