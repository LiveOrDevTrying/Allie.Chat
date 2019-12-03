namespace Allie.Chat.Lib.DTOs.ClientApplications
{
    /// <summary>
    /// A PKCE / Authorization Code Client Application data-transfer object
    /// </summary>
    public class ClientApplicationPKCEDTO : ClientApplicationDTO
    {
        /// <summary>
        /// Allowed Redirect Uris after PKCE / Authorization Code Authorization
        /// </summary>
        public string[] RedirectUris { get; set; }
        /// <summary>
        /// Allowed Uris to redirect the Application User after session logout
        /// </summary>
        public string[] PostLogoutRedirectUris { get; set; }
    }
}
