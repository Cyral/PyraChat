using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Join message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.1"/>
    public class JoinMessage : SendableMessage
    {
        public string Channel { get; }

        public JoinMessage(Channel channel)
        {
            Channel = channel.Name;
        }

        public JoinMessage(string channel)
        {
            Channel = channel;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"JOIN {Channel}");
        }
    }
}
