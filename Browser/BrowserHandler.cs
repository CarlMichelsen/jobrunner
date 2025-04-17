using Browser.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Browser;

public class BrowserHandler(ILogger<BrowserHandler> logger, IOptions<BrowserOptions> options)
{
    public async Task<T> Connect<T>(Func<IWebDriver, CancellationToken, Task<T>> callback, CancellationToken cancellationToken = default)
    {
        var browserEndpoint = options.Value.WebSocketEndpoint;
        IWebDriver driver;

        if (browserEndpoint is null)
        {
            // Local browser
            new DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
        }
        else
        {
            // Remote browser in container
            driver = new RemoteWebDriver(
                browserEndpoint, 
                new ChromeOptions());
        }
        
        try
        {
            return await callback(driver, cancellationToken);
        }
        finally
        {
            driver.Quit();
        }
    }
}
