namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_MOTD message. (372)
    /// </summary>
    public class MOTDMessage : ReceivableMessage
    {
        /// <summary>
        /// The MOTD line received.
        /// </summary>
        public string Line => BaseMessage.Parameters[1].Substring(2); //Remove "- " from the line. (RFC states: ":- <text>")

        public MOTDMessage(Message msg) : base(msg)
        {
            msg.Client.MOTDBuilder.AppendLine(Line);
            msg.Client.OnReplyMOTD(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "372";
        }
    }
}