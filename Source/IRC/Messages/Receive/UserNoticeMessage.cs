namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Notice message for users.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.3.2" />
    public class UserNoticeMessage : ReceivableMessage
    {
        public string Notice { get; }

        public UserNoticeMessage(Message msg) : base(msg)
        {
            Notice = msg.Parameters[1];
            msg.Client.OnNotice(this);
        }

        public static bool CanProcess(Message msg)
        {
            return !msg.IsChannel && msg.Type == "NOTICE";
        }
    }
}