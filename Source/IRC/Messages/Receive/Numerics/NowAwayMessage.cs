using System.Text.RegularExpressions;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_NOWAWAY message. (306)
    /// </summary>
    public class NowAwayMessage : ReceivableMessage
    {
        /// <summary>
        /// Away reason/message.
        /// </summary>
        public string Reason { get; }

        public NowAwayMessage(Message msg) : base(msg)
        {
            Reason = msg.Parameters[1];
            msg.Client.OnReplyNowAway(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "306";
        }
    }
}