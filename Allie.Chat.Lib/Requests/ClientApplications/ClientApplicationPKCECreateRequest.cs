namespace Allie.Chat.Lib.Requests.ClientApplications
{
    /// <summary>
    /// A PKCE / Authorization Code Client Application create request
    /// </summary>
    public class ClientApplicationPKCECreateRequest : ClientApplicationCreateRequest
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
