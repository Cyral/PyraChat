namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_YOURHOST message. (002)
    /// </summary>
    public class YourHostMessage : ReceivableMessage
    {
        /// <summary>
        /// Message text.
        /// Example: Your host is ___, running version ___
        /// </summary>
        public string Text => BaseMessage.Parameters[1];

        public YourHostMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyYourHost(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "002";
        }
    }
}