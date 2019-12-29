namespace Allie.Chat.Commands.Core.Interfaces
{
    public interface IParametersToken : IParameters
    {
        string WebAPIToken { get; set; }
    }
}
