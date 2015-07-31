using System.IO;

namespace Pyratron.PyraChat.IRC.Messages
{
    /// <summary>
    /// A sendable message.
    /// </summary>
    public interface SendableMessage
    {
        void Send(StreamWriter writer, Client client);
    }
}