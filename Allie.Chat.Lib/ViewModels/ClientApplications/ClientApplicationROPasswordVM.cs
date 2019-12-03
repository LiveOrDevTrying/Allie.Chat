namespace Allie.Chat.Lib.ViewModels.ClientApplications
{
    /// <summary>
    /// A Resource Owner Password Credentials Client Application ViewModel
    /// </summary>
    public class ClientApplicationROPasswordVM : ClientApplicationVM
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
