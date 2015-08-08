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
        /// Channels parted from.
        /// </summary>
        public Channel[] Channels { get; }

        public User User => BaseMessage.User;

        /// <summary>
        /// Part reason/message.
        /// </summary>
        public string Reason { get; private set; }

        public PartMessage(Message msg) : base(msg)
        {
            Reason = msg.Parameters[1];
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
