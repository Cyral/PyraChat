namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_EXCEPTLIST  message. (348)
    /// </summary>
    public class ExceptListMessage : ReceivableMessage
    {
        /// <summary>
        /// Exception list item containing mask, author, and date.
        /// </summary>
        public PermissionItem Exception { get; }

        public string Channel => BaseMessage.Parameters[1];

        public ExceptListMessage(Message msg) : base(msg)
        {
            if (msg.Parameters.Length >= 5)
                Exception = new PermissionItem(msg.Parameters[2], Utils.UnixTimeStampToDateTime(int.Parse(msg.Parameters[4])), msg.Parameters[3]);
            var channel = msg.Client.ChannelFromName(Channel);
            channel?.AddException(msg.Parameters[2]);
            msg.Client.OnReplyExceptList(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "348";
        }
    }
}