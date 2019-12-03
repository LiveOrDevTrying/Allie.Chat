namespace Allie.Chat.Lib.Requests.ClientApplications
{
    /// <summary>
    /// An Authorization Code Client Application create request
    /// </summary>
    public class ClientApplicationAuthCodeCreateRequest : ClientApplicationCreateRequest
    {
        /// <summary>
        /// Allowed Redirect Uris after Authorization Code Authorization
        /// </summary>
        public string[] RedirectUris { get; set; }
        /// <summary>
        /// Allowed Uris to redirect the Application User after session logout
        /// </summary>
        public string[] PostLogoutRedirectUris { get; set; }
        /// <summary>
        /// The front channel logout Uri for the Authorization Code Client Application
        /// </summary>
        public string FrontChannelLogoutUri { get; set; }
        /// <summary>
        /// Client secrets for the Resource Owner Password Credetials Client Application
        /// </summary>
        public string[] ClientSecrets { get; set; }
        /// <summary>
        /// The application's authorized origin (operating Uri / domain)
        /// </summary>
        public string[] AllowedCorsOrigins { get; set; }
    }
}
