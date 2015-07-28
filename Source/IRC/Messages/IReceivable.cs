using System.IO;

namespace Pyratron.PyraChat.IRC.Messages
{
    public interface IReceivable
    {
        bool CanProcess(Message message);
        string Receive(StreamReader reader, IRCClient client);
    }
}