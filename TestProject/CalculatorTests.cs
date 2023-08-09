namespace TestProject;

[TestClass]
public class CalculatorTests : PageTest
{
    [TestInitialize]
    public async Task TestInitialize()
    {
        await Page.GotoAsync("https://testsheepnz.github.io/BasicCalculator.html");
    }

    [TestMethod]
    public async Task TestMethod1()
    {
        await Page.Locator("#number1Field").FillAsync("34");
        await Page.Locator("#number2Field").FillAsync("57");

        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Calculate" }).ClickAsync();

        var result = await Page.Locator("#numberAnswerField").InputValueAsync();
        Assert.AreEqual("91", result);
    }

    [TestMethod]
    [DataRow(33, 56, 89)]
    [DataRow(34, 57, 91)]
    public async Task TestAddMethod(int a, int b, int c)
    {
        await Page.Locator("#number1Field").FillAsync(a.ToString());
        await Page.Locator("#number2Field").FillAsync(b.ToString());

        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Calculate" }).ClickAsync();

        var result = await Page.Locator("#numberAnswerField").InputValueAsync();
        Assert.AreEqual(c.ToString(), result);
    }
}