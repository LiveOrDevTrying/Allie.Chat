﻿using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.ViewModels.ClientApplications
{
    /// <summary>
    /// A Client Application base ViewModel
    /// </summary>
    public class ClientApplicationVM
    {
        /// <summary>
        /// Unique Id of the Client Application
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// The ClientId of the Client Application
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// The Client Application's name
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// The Client Application type
        /// </summary>
        public ClientApplicationType ClientApplicationType { get; set; }
        /// <summary>
        /// The Client Application type as a value
        /// </summary>
        public string ClientApplicationTypeValue { get; set; }
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
