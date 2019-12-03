namespace Allie.Chat.Lib.Requests.ClientApplications
{
    /// <summary>
    /// A Resource Owner Password Credentials Client Application create request
    /// </summary>
    public class ClientApplicationROPasswordCreateRequest : ClientApplicationCreateRequest
    {
        /// <summary>
        /// Client secrets to create for the Resource Owner Password Credetials Client Application
        /// </summary>
        public string[] ClientSecrets { get; set; }
        /// <summary>
        /// The application's authorized origin (operating Uri / domain)
        /// </summary>
        public string[] AllowedCorsOrigins { get; set; }
    }
}
