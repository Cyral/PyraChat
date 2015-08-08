using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_WHOIS____ messages. (311-319 excluding 315 and 318)
    /// Handles WHOIS related responses.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.6.2" />
    public class WhoisMessage : ReceivableMessage
    {
        public WhoisMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyWhois(this);
        }

        public static bool CanProcess(Message msg)
        {
            int numeric;
            if (int.TryParse(msg.Type, out numeric))
            {
                if (numeric >= 311 && numeric < 319 && numeric != 315 && numeric != 318)
                    return true;
            }
            return false;
        }
    }
}