namespace Allie.Chat.Lib.DTOs.ClientApplications
{
    /// <summary>
    /// A Resource Owner Password Credentials Client Application data-transfer object
    /// </summary>
    public class ClientApplicationROPasswordDTO : ClientApplicationDTO
    {
        /// <summary>
        /// The number of secrets registered to the Resource Owner Password Credentials Client Application
        /// </summary>
        public int SecretsCount { get; set; }
        /// <summary>
        /// The application's authorized origin (operating Uri / domain)
        /// </summary>
        public string[] AllowedCorsOrigins { get; set; }
    }
}
