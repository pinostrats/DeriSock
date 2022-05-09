using System;

namespace DeriSock.JsonRpc;

/// <summary>
/// A factory class that creates a <see cref="Socks5Socket"/>
/// </summary>
public class Socks5WebSocketFactory : IWebSocketFactory
{
  private readonly Socks5Settings _socks5Settings;

  /// <summary>
  /// The constructor method
  /// </summary>
  /// <param name="socks5Settings">The settings class</param>
  /// <exception cref="ArgumentNullException">Throws if the settings object is null</exception>
  public Socks5WebSocketFactory(Socks5Settings socks5Settings)
  {
    _socks5Settings = socks5Settings ?? throw new ArgumentNullException(nameof(socks5Settings));
  }

  /// <inheritdoc />
  public IWebSocket Create()
  {
    return new Socks5Socket(_socks5Settings); 
  }
}
