using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// End of names message. (Second message after RPL_NAMEREPLY) (RPL_ENDOFNAMES).
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.5" />
    public class EndOfNamesMessage : ReceivableMessage
    {
        public Channel Channel { get; }

        public EndOfNamesMessage(Message msg) : base(msg)
        {
            Channel = msg.Client.ChannelFromName(msg.Parameters[1]);
            if (Channel != null)
                msg.Client.OnReplyEndOfNames(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "366";
        }
    }
}