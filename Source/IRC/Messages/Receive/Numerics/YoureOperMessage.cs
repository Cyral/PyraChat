namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_YOUREOPER message. (381)
    /// </summary>
    public class YoureOperMessage : ReceivableMessage
    {
        public string Info => BaseMessage.Parameters[1];

        public YoureOperMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyYoureOper(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "381";
        }
    }
}