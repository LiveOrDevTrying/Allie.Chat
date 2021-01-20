namespace Allie.Chat.Interfaces
{
    public interface IParametersToken : IParameters
    {
        string WebAPIToken { get; set; }
    }
}
