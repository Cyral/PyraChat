using System;
using System.Linq;
using Microsoft.Win32;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Part message. (PART)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.2"/>
    public class PartMessage : ReceivableMessage
    {
        /// <summary>
        /// Part reason/message.
        /// </summary>
        public string Reason => BaseMessage.Parameters[1];

        /// <summary>
        /// Channels parted from.
        /// </summary>
        public Channel[] Channels { get; }

        public User User => BaseMessage.User;

        public PartMessage(Message msg) : base(msg)
        {
            var channelNames = msg.Destination.Split(' ');
            Channels = channelNames.Select(channelName => msg.Client.ChannelFromName(channelName)).ToArray();
            foreach (var channel in Channels)
            {
                channel.RemoveUser(msg.User);
                channel.OnUserPart(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "PART";
        }
    }
}
