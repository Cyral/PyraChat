using System.Text.RegularExpressions;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// A numeric (non-error) message. (0-399)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-5.2"/>
    public class NumericMessage : ReceivableMessage
    {
        public string[] Parameters => BaseMessage.Parameters;
        public string Destination => BaseMessage.Destination;
        public User User => BaseMessage.User;
        public Channel Channel => BaseMessage.Channel;
        public int MessageCode { get; }

        public NumericMessage(Message msg) : base(msg)
        {
            MessageCode = int.Parse(msg.Type);
            msg.Client.OnNumeric(this);
        }

        public static bool CanProcess(Message msg)
        {
            int numeric;
            if (int.TryParse(msg.Type, out numeric))
            {
                if (numeric >= 0 && numeric < 399)
                    return true;
            }
            return false;
        }
    }
}