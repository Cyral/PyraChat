namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_INVITELIST message. (346)
    /// </summary>
    public class InviteListMessage : ReceivableMessage
    {
        /// <summary>
        /// Invite list item containing mask, author, and date.
        /// </summary>
        public PermissionItem Invite { get; }

        public string Channel => BaseMessage.Parameters[1];

        public InviteListMessage(Message msg) : base(msg)
        {
            if (msg.Parameters.Length >= 5)
                Invite = new PermissionItem(msg.Parameters[2], Utils.UnixTimeStampToDateTime(int.Parse(msg.Parameters[4])), msg.Parameters[3]);
            var channel = msg.Client.ChannelFromName(Channel);
            channel?.AddInvite(msg.Parameters[2]);
            msg.Client.OnReplyInviteList(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "346";
        }
    }
}