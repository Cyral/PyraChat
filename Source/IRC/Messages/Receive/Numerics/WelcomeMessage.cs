namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// Welcome message. (001)
    /// </summary>
    public class WelcomeMessage : IReceivable
    {
        public void Process(Message msg)
        {
            msg.Client.OnConnect();
            msg.Client.OnWelcome(msg.Parameters[1]);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "001";
        }
    }
}