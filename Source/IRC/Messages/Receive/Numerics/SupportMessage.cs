using System;
using System.Collections.Generic;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_ISUPPORT message. (005)
    /// </summary>
    public class SupportMessage : ReceivableMessage
    {
        /// <summary>
        /// Dictionary of parameters and supported values.
        /// </summary>
        /// <see cref="http://www.irc.org/tech_docs/005.html" />
        public Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        public SupportMessage(Message msg) : base(msg)
        {
            //Parse ISUPPORT parameters.
            for (var i = 1; i < msg.Parameters.Length; i++)
            {
                if (msg.Parameters[i] == null)
                    break;

                var param = msg.Parameters[i].Split('=');
                Parameters.Add(param[0], param.Length == 1 ? null : param[1]);
            }
            msg.Client.OnReplyISupport(this);
        }

        public static bool CanProcess(Message msg)
        {
            //PREFIX differentiates between depreciated RPL_BOUNCE method with the same ID.
            //See https://www.alien.net.au/irc/irc2numerics.html
            return msg.Type == "005" && !msg.Parameters[1].StartsWith("Try Server", StringComparison.OrdinalIgnoreCase);
        }
    }
}