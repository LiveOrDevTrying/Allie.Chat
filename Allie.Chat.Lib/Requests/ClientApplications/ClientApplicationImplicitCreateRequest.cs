namespace Allie.Chat.Lib.Requests.ClientApplications
{
    /// <summary>
    /// An Implicit Client Application create request
    /// </summary>
    public class ClientApplicationImplicitCreateRequest : ClientApplicationCreateRequest
    {
        /// <summary>
        /// Allowed Redirect Uris after Implicit Authorization
        /// </summary>
        public string[] RedirectUris { get; set; }
        /// <summary>
        /// Allowed Uris to redirect the Application User after session logout
        /// </summary>
        public string[] PostLogoutRedirectUris { get; set; }
        /// <summary>
        /// The application's authorized origin (operating Uri / domain)
        /// </summary>
        public string[] AllowedCorsOrigins { get; set; }
    }
}
