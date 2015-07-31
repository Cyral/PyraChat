using System.Text;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_MOTDSTART message. (375)
    /// </summary>
    public class MOTDStartMessage : ReceivableMessage
    {
        /// <summary>
        /// Start of the message of the day.
        /// </summary>
        public string Text => BaseMessage.Parameters[1];

        public MOTDStartMessage(Message msg) : base(msg)
        {
            msg.Client.MOTDBuilder = new StringBuilder();
            msg.Client.OnReplyMOTDStart(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "375";
        }
    }
}