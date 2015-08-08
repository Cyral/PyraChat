using System.Text.RegularExpressions;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_TIME message. (391)
    /// </summary>
    public class TimeMessage : ReceivableMessage
    {
        public string Server => BaseMessage.Parameters[1];

        public string LocalTime => BaseMessage.Parameters[2];

        public TimeMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyTime(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "391";
        }
    }
}