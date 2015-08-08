namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Channel mode message. (MODE)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.3" />
    public class ChannelModeMessage : ReceivableMessage
    {
        public User User => BaseMessage.User;
        private string Modes => BaseMessage.Parameters[1];

        public ChannelModeMessage(Message msg) : base(msg)
        {
            var paramIndex = 2;
            if (msg.Channel.Name == msg.Destination)
            {
                var add = false;
                foreach (var character in msg.Parameters[1])
                {
                    if (character.Equals('+'))
                        add = true;
                    else if (character.Equals('-'))
                        add = false;
                    else if (add) paramIndex += msg.Channel.AddMode(msg.Client, msg.Channel.Name, character, msg.Parameters[paramIndex]);
                    else paramIndex += msg.Channel.RemoveMode(msg.Client, msg.Channel.Name, character, msg.Parameters[paramIndex]);
                }
                msg.Channel.OnChannelMode(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "MODE" && msg.IsChannel;
        }
    }
}