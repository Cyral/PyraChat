using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_CHANNELMODEIS
    /// </summary>
    public class ChannelModeIsMessage : ReceivableMessage
    {
        public string Modes => BaseMessage.Parameters[2];

        public ChannelModeIsMessage(Message msg) : base(msg)
        {
            var paramIndex = 3;
            var channel = msg.Client.ChannelFromName(msg.Parameters[1]);
            if (channel != null)
            {
                var add = false;
                foreach (var character in msg.Parameters[2])
                {
                    if (character.Equals('+'))
                        add = true;
                    else if (character.Equals('-'))
                        add = false;
                    else if (add) paramIndex += channel.AddMode(msg.Client, channel.Name, character, msg.Parameters[paramIndex]);
                    else paramIndex += channel.RemoveMode(msg.Client, channel.Name, character, msg.Parameters[paramIndex]);
                }
                channel.OnReplyChannelModeIs(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "324";
        }
    }
}