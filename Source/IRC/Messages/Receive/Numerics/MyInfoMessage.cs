namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_MYINFO message. (004)
    /// </summary>
    public class MyInfoMessage : ReceivableMessage
    {
        public string ServerName => BaseMessage.Parameters[1];
        public string Version => BaseMessage.Parameters[2];
        public string AvailableUserModes => BaseMessage.Parameters[3];
        public string AvailableChannelModes => BaseMessage.Parameters[4];

        public MyInfoMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyMyInfo(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "004";
        }
    }
}