using System;
using System.Linq;
using Pyratron.PyraChat.IRC.Messages.Send;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Join message. (JOIN)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.1"/>
    public class JoinMessage : ReceivableMessage
    {
        public Channel Channel { get; }

        public User User => BaseMessage.User;

        public JoinMessage(Message msg) : base(msg)
        {
            Channel = msg.Client.ChannelFromName(msg.Destination);
            var user = User;
            if (!msg.Client.Channels.Any(c => c.Name.Equals(msg.Destination, StringComparison.OrdinalIgnoreCase)))
            {
                //Add initial channel and user
                msg.Client.User.Host = BaseMessage.User.Host;
                user = msg.Client.User;
                msg.Client.Channels.Add(Channel);
            }

            if (user == msg.Client.User) //If user joined is ourself
            {
                msg.Client.OnChannelJoin(this);
                msg.Channel.AddUser(user);
                msg.Client.Send(new WhoMessage(Channel.Name)); //Request information about users in this channel, such as away status.
            }
            else
            {
                msg.Channel.AddUser(user);
                msg.Channel.OnUserJoin(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "JOIN";
        }
    }
}
