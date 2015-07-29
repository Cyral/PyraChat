using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Private message (PRIVMSG).
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.3.1" />
    public class PrivateMessage : IReceivable
    {
        public void Process(Message msg)
        {
            var target = msg.Destination;
            var message = msg.Parameters[1];

            if (msg.IsChannel)
            {

            }
            else
            {
                
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "PRIVMSG";
        }
    }
}