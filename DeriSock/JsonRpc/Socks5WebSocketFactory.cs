using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace DeriSock.JsonRpc;

/// <summary>
/// A factory class that creates a <see cref="Socks5Socket"/>
/// </summary>
public class Socks5WebSocketFactory : IWebSocketFactory
{
  private readonly Socks5Settings _socks5Settings;
  private readonly ILogger _logger;

  /// <summary>
  /// The constructor method
  /// </summary>
  /// <param name="socks5Settings">The settings class</param>
  /// <param name="logger">Logger instance</param>
  /// <exception cref="ArgumentNullException">Throws if the settings object is null</exception>
  public Socks5WebSocketFactory(Socks5Settings socks5Settings, ILogger logger)
  {
    _socks5Settings = socks5Settings ?? throw new ArgumentNullException(nameof(socks5Settings));
    _logger = logger.ForContext<Socks5WebSocketFactory>();
  }

  /// <inheritdoc />
  public IWebSocket Create()
  {
    if (_logger?.IsEnabled(LogEventLevel.Information) ?? false)
    {
      _logger.Information("Using Socks5 Address: {Address} Username: {Username}", 
        _socks5Settings.Address,
        _socks5Settings.Username);
    }
    return new Socks5Socket(_socks5Settings); 
  }
}
