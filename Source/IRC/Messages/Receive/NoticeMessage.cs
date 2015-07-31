namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Notice message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.3.2" />
    public class NoticeMessage : ReceivableMessage
    {
        public string Notice { get; }

        public NoticeMessage(Message msg) : base(msg)
        {
            Notice = msg.Parameters[1];
            if (msg.IsChannel)
                msg.Channel.OnNotice(this);
            else
                msg.Client.OnNotice(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "NOTICE";
        }
    }
}