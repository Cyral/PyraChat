namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// WALLOPS message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-4.6" />
    public class OperwallMessage : ReceivableMessage
    {
        public string Message => BaseMessage.Parameters[1];

        public OperwallMessage(Message msg) : base(msg)
        {
            msg.Client.OnOperwall(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "WALLOPS";
        }
    }
}