using System;
using System.Text.RegularExpressions;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_ISON message. (303)
    /// </summary>
    public class IsOnMessage : ReceivableMessage
    {
        /// <summary>
        /// Nicknames from the request who are online.
        /// </summary>
        public string[] Nicks { get; }

        public IsOnMessage(Message msg) : base(msg)
        {
            Nicks = msg.Parameters[1].Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
            msg.Client.OnReplyIsOn(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "303";
        }
    }
}