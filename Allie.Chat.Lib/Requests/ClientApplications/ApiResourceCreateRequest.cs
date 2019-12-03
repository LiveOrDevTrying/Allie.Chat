using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.ClientApplications
{
    /// <summary>
    /// An Api Resource create or update request
    /// </summary>
    public class ApiResourceCreateRequest : BaseRequest
    {
        /// <summary>
        /// The Api Resource's name
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// The Api Resource's display name
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Array of scopes allowed by the Api Resource
        /// </summary>
        public string[] Scopes { get; set; }
        /// <summary>
        /// Api secrets to create for the Api Resource
        /// </summary>
        public string[] ApiSecrets { get; set; }
    }
}
