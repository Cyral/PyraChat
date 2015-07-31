namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// Message of the day (RPL_MOTDEND) message. (376)
    /// </summary>
    public class MOTDEndMessage : ReceivableMessage
    {
        /// <summary>
        /// The final message of the day.
        /// </summary>
        public string MOTD { get; }

        public MOTDEndMessage(Message msg) : base(msg)
        {
            MOTD = msg.Client.MOTDBuilder.ToString();
            msg.Client.OnReplyMOTDEnd(this);
            msg.Client.MOTDBuilder.Clear();
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "376";
        }
    }
}