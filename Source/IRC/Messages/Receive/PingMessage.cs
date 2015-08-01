namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Ping message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.7.2"/>
    public class PingMessage : ReceivableMessage
    {
        /// <summary>
        /// Extra message content sent by the ping message.
        /// </summary>
        public string Extra { get; }

        public PingMessage(Message msg) : base(msg)
        {
            Extra = msg.Parameters[0];
            msg.Client.OnPing(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "PING";
        }
    }
}
