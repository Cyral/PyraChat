namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// User mode message. (MODE)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.1" />
    public class UserModeMessage : ReceivableMessage
    {
        public User User => BaseMessage.User;
        private string Modes => BaseMessage.Parameters[1];

        public UserModeMessage(Message msg) : base(msg)
        {
            if (msg.User.Nick == msg.Destination) //User and nick sent must match (RFC spec)
            {
                var add = false;
                foreach (var character in msg.Parameters[1])
                {
                    if (character.Equals('+'))
                        add = true;
                    else if (character.Equals('+'))
                        add = false;
                    else if (add) msg.User.AddMode(msg.Client, character);
                    else msg.User.RemoveMode(msg.Client, character);
                }
                msg.Client.OnUserMode(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "MODE" && !msg.IsChannel;
        }
    }
}