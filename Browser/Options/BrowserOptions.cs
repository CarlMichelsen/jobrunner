using System.ComponentModel.DataAnnotations;
using Interface.Options;

namespace Browser.Options;

public class BrowserOptions : IOptionsImpl
{
    public static string SectionName => "Browser";
    
    /// <summary>
    /// Gets Websocket endpoint if the browser to be used is not running on the same machine in dev-mode.
    /// </summary>
    public Uri? WebSocketEndpoint { get; init; }
}