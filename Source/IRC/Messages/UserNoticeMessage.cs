namespace Pyratron.PyraChat.IRC.Messages
{
    /// <summary>
    /// Notice message for users.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.3.2" />
    public class UserNoticeMessage : IReceivable
    {
        public void Receive(Message msg)
        {
            var notice = msg.Parameters[1];
            msg.Client.OnNotice(msg.User, notice);
        }

        public static bool CanProcess(Message msg)
        {
            return !msg.IsChannel && msg.Type == "NOTICE";
        }
    }
}