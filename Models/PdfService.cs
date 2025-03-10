using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Threading.Tasks;

namespace ERP.Models
{
	public class PdfService
	{
		public async Task<byte[]> GeneratePdfFromHtml(string htmlContent)
		{
			// Specify the path to the installed Google Chrome executable
			var browserOptions = new LaunchOptions
			{
				Headless = true, // Run Chrome in headless mode
				ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
				Timeout = 300000 // Increase timeout to handle complex operations
			};

			// Launch the browser
			using (var browser = await Puppeteer.LaunchAsync(browserOptions))
			{
				// Create a new page
				using (var page = await browser.NewPageAsync())
				{
					// Set the page content
					await page.SetContentAsync(htmlContent);

					// Generate PDF from the page content
					var pdfOptions = new PdfOptions
					{
						Format = PaperFormat.A4,
						PrintBackground = true
					};
					var pdfBytes = await page.PdfDataAsync(pdfOptions);

					return pdfBytes;
				}
			}
		}
	}

}
