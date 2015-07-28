using System.IO;

namespace Pyratron.PyraChat.IRC.Messages
{
    public interface IReceivable
    {
        void Process(Message msg);
    }
}