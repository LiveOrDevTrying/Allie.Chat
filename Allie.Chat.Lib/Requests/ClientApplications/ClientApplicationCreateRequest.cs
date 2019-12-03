using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.ClientApplications
{
    /// <summary>
    /// A Client Application base create or update request
    /// </summary>
    public abstract class ClientApplicationCreateRequest : BaseRequest
    {
        /// <summary>
        /// The ClientId of the Client Application
        /// </summary>
        [Required]
        public string ClientId { get; set; }
        /// <summary>
        /// The Client Application's name
        /// </summary>
        [Required]
        public string ClientName { get; set; }
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
