namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// Version message. (351)
    /// </summary>
    public class VersionMessage : ReceivableMessage
    {
        public string Version => BaseMessage.Parameters[0];

        public VersionMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyVersion(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "351";
        }
    }
}