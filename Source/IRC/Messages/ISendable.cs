using System.IO;

namespace Pyratron.PyraChat.IRC.Messages
{
    public interface ISendable
    {
        void Send(StreamWriter writer, Client client);
    }
}