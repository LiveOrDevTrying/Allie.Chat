namespace Allie.Chat.Lib.Enums
{
    /// <summary>
    /// The Route types
    /// </summary>
    public enum RouteType
    {
        /// <summary>
        /// An inbound route
        /// </summary>
        Inbound,
        /// <summary>
        /// An outbound route
        /// </summary>
        Outbound
    }

    /// <summary>
    /// The Path types
    /// </summary>
    public enum PathType
    {
        /// <summary>
        /// No server associated to Path e.g. Tcp or Websocket
        /// </summary>
        None,
        /// <summary>
        /// Server associated to Path e.g. Twitch
        /// </summary>
        Server,
        /// <summary>
        /// Server and Channel associated to Path e.g. Discord inbound
        /// </summary>
        Channel,
        /// <summary>
        /// Server and Channel associated to Outbound Discord Path
        /// </summary>
        ChannelWebhook
    }

    /// <summary>
    /// The Provider type
    /// </summary>
    public enum ProviderType
    {
        /// <summary>
        /// Twitch.com
        /// </summary>
        Twitch,
        /// <summary>
        /// Discord.com
        /// </summary>
        Discord,
        /// <summary>
        /// Allie.Chat Tcp Provider
        /// </summary>
        Tcp,
        /// <summary>
        /// Allie.Chat Websocket Provider
        /// </summary>
        Websocket
    }

    /// <summary>
    /// The available Client Application Types
    /// </summary>
    public enum ClientApplicationType
    {
        /// <summary>
        /// Implicit flow authorization
        /// </summary>
        Implicit,
        /// <summary>
        /// Authorization Code flow authorization
        /// </summary>
        AuthCode,
        /// <summary>
        /// Resource Owner Password flow authorization
        /// </summary>
        ROPassword,
        /// <summary>
        /// Native App / PKCE Authorization Code flow authorization
        /// </summary>
        NativePKCE
    }
}
