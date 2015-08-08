using System;
using System.Linq;
using Microsoft.Win32;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Quit message. (QUIT)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.1.7"/>
    public class QuitMessage : ReceivableMessage
    {
        /// <summary>
        /// Quit reason/message.
        /// </summary>
        public string Reason => BaseMessage.Parameters[0];

        public QuitMessage(Message msg) : base(msg)
        {
            foreach (var channel in msg.User.Channels.ToList())
                channel.RemoveUser(msg.User);
            msg.Client.OnQuit(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "QUIT";
        }
    }
}
