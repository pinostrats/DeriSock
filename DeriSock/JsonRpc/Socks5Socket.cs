using System;
using System.Net;

namespace DeriSock.JsonRpc;

/// <summary>
/// A Socks5 socket connection that routes traffic thru a proxy server
/// </summary>
public class Socks5Socket : DefaultWebSocket
{
  
  /// <summary>
  /// The constructor method
  /// </summary>
  /// <param name="socks5Settings">The settings class</param>
  /// <exception cref="ArgumentNullException">Throws if the settings object is null</exception>
  public Socks5Socket(Socks5Settings socks5Settings)
  {
    if (socks5Settings == null)
    {
      throw new ArgumentNullException(nameof(socks5Settings));
    }

    _socket.Options.Proxy = new WebProxy(socks5Settings.Address);
    _socket.Options.Proxy.Credentials = new NetworkCredential() 
    { 
      UserName = socks5Settings.Username, 
      Password = socks5Settings.Password
    };
  }
}
