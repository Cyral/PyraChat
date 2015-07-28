namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Ping message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.7.2"/>
    public class PingMessage : IReceivable
    {
        public void Process(Message msg)
        {
            var message = msg.Parameters[1];
            msg.Client.OnPing(message);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "PING";
        }
    }
}
