namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Notice message for channels.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.3.2" />
    public class ChannelNoticeMessage : ReceivableMessage
    {
        public string Notice { get; }

        public ChannelNoticeMessage(Message msg) : base(msg)
        {
            Notice = msg.Parameters[1];
            msg.Channel.OnNotice(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.IsChannel && msg.Type == "NOTICE";
        }
    }
}