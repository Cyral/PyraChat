using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Quit message. (QUIT)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.1.7" />
    public class QuitMessage : ReceivableMessage
    {
        /// <summary>
        /// Quit reason/message.
        /// </summary>
        public string Reason => BaseMessage.Parameters[0];

        public User User => BaseMessage.User;

        public QuitMessage(Message msg) : base(msg)
        {
            msg.Client.OnQuit(this); // Call event before quit so handlers can get a list of channels.
            foreach (var channel in msg.User.Channels.ToList())
                channel.RemoveUser(msg.User);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "QUIT";
        }
    }
}