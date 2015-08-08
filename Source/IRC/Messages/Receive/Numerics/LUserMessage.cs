namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_LUSER____ messages. (251-255)
    /// </summary>
    public class LUserMessage : ReceivableMessage
    {
        public LUserMessage(Message msg) : base(msg)
        {
             msg.Client.OnReplyLUser(this);
        }

        public static bool CanProcess(Message msg)
        {
            int numeric;
            if (int.TryParse(msg.Type, out numeric))
            {
                if (numeric >= 251 && numeric <= 255)
                    return true;
            }
            return false;
        }
    }
}