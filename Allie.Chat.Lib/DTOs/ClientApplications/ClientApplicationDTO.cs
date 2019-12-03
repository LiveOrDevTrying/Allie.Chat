namespace Allie.Chat.Lib.DTOs.ClientApplications
{
    /// <summary>
    /// A Client Application base data-transfer object
    /// </summary>
    public class ClientApplicationDTO
    {
        /// <summary>
        /// Unique Id of the Client Application
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The ClientId of the Client Application
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// The Client Application's name
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// The Client Application's authorization types
        /// </summary>
        public string[] AuthTypes { get; set; }
        /// <summary>
        /// The Client Application type
        /// </summary>
        public string ClientApplicationType { get; set; }
        /// <summary>
        /// The Client Application's Uri
        /// </summary>
        public string ClientUri { get; set; }
        /// <summary>
        /// Array of scopes allowed by the Client Application
        /// </summary>
        public string[] Scopes { get; set; }
        /// <summary>
        /// Will oauth tokens be used for offline access?
        /// </summary>
        public bool AllowOfflineAccess { get; set; }
        /// <summary>
        /// Is consent required?
        /// </summary>
        public bool RequireConsent { get; set; }
        /// <summary>
        /// Is local login enabled?
        /// </summary>
        public bool EnableLocalLogin { get; set; }
    }
}
