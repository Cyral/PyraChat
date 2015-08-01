namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// Names message (RPL_NAMES).
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.5" />
    public class NamesMessage : ReceivableMessage
    {
        public NamesMessage(Message msg) : base(msg)
        {

        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "353";
        }
    }
}