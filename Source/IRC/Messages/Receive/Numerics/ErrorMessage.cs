using System.Text.RegularExpressions;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// An error message. (400-599)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-5.2"/>
    public class ErrorMessage : ReceivableMessage
    {
        /// <summary>
        /// Error message after :
        /// </summary>
        public string Message => BaseMessage.Parameters[BaseMessage.Parameters.Length - 1];

        public int ErrorCode { get; }

        public ErrorMessage(Message msg) : base(msg)
        {
            ErrorCode = int.Parse(msg.Type);
            msg.Client.OnError(this);
        }

        public static bool CanProcess(Message msg)
        {
            int numeric;
            if (int.TryParse(msg.Type, out numeric))
            {
                if (numeric >= 400 && numeric < 600)
                    return true;
            }
            return false;
        }
    }
}