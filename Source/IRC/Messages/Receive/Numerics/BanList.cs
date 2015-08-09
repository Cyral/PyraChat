namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_BANLIST message. (367)
    /// </summary>
    public class BanListMessage : ReceivableMessage
    {
        /// <summary>
        /// Ban list item containing mask, author, and date.
        /// </summary>
        public PermissionItem Ban { get; }

        public string Channel => BaseMessage.Parameters[1];

        public BanListMessage(Message msg) : base(msg)
        {
            if (msg.Parameters.Length >= 5)
                Ban = new PermissionItem(msg.Parameters[2], Utils.UnixTimeStampToDateTime(int.Parse(msg.Parameters[4])), msg.Parameters[3]);
            var channel = msg.Client.ChannelFromName(Channel);
            channel?.AddBan(msg.Parameters[2]);
            msg.Client.OnReplyBanList(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "367";
        }
    }
}