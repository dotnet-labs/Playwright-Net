namespace TestProject;

[TestClass]
public class ApiRequestTests : PageTest
{
    private const string SampleResponseForValidMfk = @"1
    Valid MFK
    { GRANT=Not Assigned, FUND=Stores Srvcs Revolving Dpt Svc, ORG=Information Technology Service, DEPT=ITS Admin Information Systems, IACT=Travel Out State Fac and Staff, COSTCENTER=Not Assigned, FUNCTION=Not Assigned }
    ";

    private const string SampleResponseForInvalidMfk = @"0
    Invalid MFK - FUND DOES NOT EXIST
    { GRANT=Not Assigned, FUND=Not Found, ORG=Tippie College of Business, DEPT=Not Found, IACT=Not Found, COSTCENTER=Hamilton Dennis, FUNCTION=Not Found }
    ";

    [TestMethod]
    public async Task MfkValidationTests()
    {
        await Page.GotoAsync("https://changhuixu.github.io/uiowa-mfk-project/mfk-validations");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Set an MFK Value" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Validate" }).ClickAsync();

        await Page.WaitForResponseAsync(response => response.Url.StartsWith("https://apps.its.uiowa.edu/mfk/api-singleDesc.jsp"));
        //Console.WriteLine(await response.TextAsync());
        var result = await Page.Locator("pre").First.TextContentAsync();
        Console.WriteLine(result);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("\"statusCode\": 1,"));
        Assert.IsTrue(result.Contains("\"statusMessage\": \"Valid MFK\","));
    }

    [TestMethod]
    [DataRow(SampleResponseForValidMfk, 1, "Valid MFK")]
    [DataRow(SampleResponseForInvalidMfk, 0, "Invalid MFK - FUND DOES NOT EXIST")]
    public async Task MfkValidationMockApiTests(string apiResponse, int displayStatusCode, string displayStatusMessage)
    {
        await Page.RouteAsync("https://apps.its.uiowa.edu/mfk/api-singleDesc.jsp*", async route =>
        {
            await route.FulfillAsync(new RouteFulfillOptions
            {
                ContentType = "text/plain",
                Body = apiResponse
            });
        });

        await Page.GotoAsync("https://changhuixu.github.io/uiowa-mfk-project/mfk-validations");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Set an MFK Value" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Validate" }).ClickAsync();

        var result = await Page.Locator("pre").First.TextContentAsync();
        Console.WriteLine(result);
        Assert.IsTrue(result?.Contains($"\"statusCode\": {displayStatusCode},"));
        Assert.IsTrue(result?.Contains($"\"statusMessage\": \"{displayStatusMessage}\","));
    }
}