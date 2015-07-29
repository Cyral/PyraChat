namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Join message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.1"/>
    public class JoinMessage : IReceivable
    {
        public void Process(Message msg)
        {
            if (msg.User == msg.Client.User) //If user joined is ourself
                msg.Client.OnChannelJoin(msg.Channel);
            else
            {
                //TODO: Request info of user (WHO)
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "PING";
        }
    }
}
