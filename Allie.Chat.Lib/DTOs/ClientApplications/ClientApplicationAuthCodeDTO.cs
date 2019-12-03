namespace Allie.Chat.Lib.DTOs.ClientApplications
{
    /// <summary>
    /// An Authorization Code Client Application data-transfer object
    /// </summary>
    public class ClientApplicationAuthCodeDTO : ClientApplicationDTO
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
        /// The number of secrets registered to the Authorization Code Client Application
        /// </summary>
        public int SecretsCount { get; set; }
        /// <summary>
        /// The application's authorized origin (operating Uri / domain)
        /// </summary>
        public string[] AllowedCorsOrigins { get; set; }
    }
}
