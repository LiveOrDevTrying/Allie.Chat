namespace Allie.Chat.Lib.ViewModels.ClientApplications
{
    /// <summary>
    /// The Implicit Client Application ViewModel
    /// </summary>
    public class ClientApplicationImplicitVM : ClientApplicationVM
    {
        /// <summary>
        /// The Implicit Client Application's authorized redirect Uris
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
