using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_UMODEIS
    /// </summary>
    public class UModeIsMessage : ReceivableMessage
    {
        public string Modes => BaseMessage.Parameters[1];

        public UModeIsMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyUModeIs(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "221";
        }
    }
}