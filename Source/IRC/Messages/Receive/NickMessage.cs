using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Nick message (NICK).
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.3.1" />
    public class NickMessage : ReceivableMessage
    {
        /// <summary>
        /// The user's new nickname.
        /// </summary>
        public string Nick => BaseMessage.Parameters[0];

        public string OldNick { get; }

        public User User => BaseMessage.User;

        public NickMessage(Message msg) : base(msg)
        {
            OldNick = User.Nick;
            User.Nick = Nick;
            msg.Client.OnNick(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "NICK";
        }
    }
}