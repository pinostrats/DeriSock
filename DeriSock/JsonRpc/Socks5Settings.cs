using System;

namespace DeriSock.JsonRpc;

/// <summary>
/// The socks5 proxy settings
/// </summary>
public class Socks5Settings
{
  /// <summary>
  /// The proxy address with protocol prefix and port, e.g. socks5://amsterdam.nl.socks.nordhold.net:1080
  /// </summary>
  public Uri Address { get; set; }
  /// <summary>
  /// The username
  /// </summary>
  public string Username { get; set; }
  /// <summary>
  /// The password
  /// </summary>
  public string Password { get; set; }
}
