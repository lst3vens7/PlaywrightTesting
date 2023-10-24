using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;

namespace PlaywrightTesting
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public async Task Test1()
		{
			using var playwright = await Playwright.CreateAsync();
			await using var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
			{
				Headless = false,
				SlowMo = 500,
				Timeout = 8000
			});

			var context = await browser.NewContextAsync(new BrowserNewContextOptions()
			{
				RecordVideoDir = "video/",
				RecordVideoSize = new RecordVideoSize()
				{
					Width = 1920,
					Height = 1080
				},
				ViewportSize = new ViewportSize()
				{
					Width = 1920,
					Height = 1080
				}
			});

			var Page = await context.NewPageAsync();

			await Page.SetViewportSizeAsync(1920, 1080);

			await Page.GotoAsync("https://www.google.com/");
			await Page.WaitForLoadStateAsync();

			IElementHandle? element = await Page.QuerySelectorAsync("text='Accept all'");

			if (element is not null)
			{
				await element.ClickAsync();
				await element.WaitForElementStateAsync(ElementState.Hidden);
				await Task.Delay(TimeSpan.FromSeconds(2.0));
			}

			await Page.TypeAsync("[name='q']", ".net core");
			await Page.Keyboard.PressAsync("Enter");

			await Page.WaitForSelectorAsync("id=appbar");

			await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "Screenshots\\Net2.png" });
			await Page.ClickAsync("a:has-text(\".NET\")");
			await Task.Delay(TimeSpan.FromSeconds(2.0));
			IElementHandle? button = await Page.QuerySelectorAsync("text='Accept'");
			if (button is not null)
			{
				await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "Screenshots\\Net3.png" });
				await button.ClickAsync();
				await Task.Delay(TimeSpan.FromSeconds(2.0));
			}

			await Task.Delay(TimeSpan.FromSeconds(2.0));
			await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "Screenshots\\Net4.png" });
			await Page.SetViewportSizeAsync(1920, 1080);

			await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "Screenshots\\Net5.png" });
			await Page.SetViewportSizeAsync(1920, 1080);
			await Task.Delay(TimeSpan.FromSeconds(5.0));
			var isExist = await Page.Locator(selector: "text='Performance'").IsVisibleAsync();

			Assert.That(isExist, Is.False);
			await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "Screenshots\\Net6.png" });
			await Page.SetViewportSizeAsync(1920, 1080);

			await Task.Delay(TimeSpan.FromSeconds(5.0));
			await context.CloseAsync();
		}
	}
}