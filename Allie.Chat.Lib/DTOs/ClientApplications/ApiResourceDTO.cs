namespace Allie.Chat.Lib.DTOs.ClientApplications
{
    /// <summary>
    /// An Api Resource data-transfer object
    /// </summary>
    public class ApiResourceDTO : BaseDTO
    {
        /// <summary>
        /// The Id of the Api Resource
        /// </summary>
        public new int Id { get; set; }
        /// <summary>
        /// The Api Resource's name
        /// </summary>
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
        /// The number of secrets registered to the Api Resource
        /// </summary>
        public int SecretCount { get; set; }
    }
}
