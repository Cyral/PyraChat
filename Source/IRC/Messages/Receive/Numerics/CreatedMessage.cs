namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_CREATED message. (003)
    /// </summary>
    public class CreatedMessage : ReceivableMessage
    {
        /// <summary>
        /// Message text.
        /// Example: This server was created ___
        /// </summary>
        public string Text => BaseMessage.Parameters[1];

        public CreatedMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyCreated(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "003";
        }
    }
}