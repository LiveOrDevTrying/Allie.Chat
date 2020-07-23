namespace Allie.Chat.Commands.Service.Interfaces
{
    public interface IParametersToken : IParameters
    {
        string WebAPIToken { get; set; }
    }
}
