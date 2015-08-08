using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Channel mode message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.1" />
    public class ChannelModeMessage : SendableMessage
    {
        public string Channel { get; }

        /// <summary>
        /// Channel modes as a string, Ex: +b
        /// </summary>
        public string Modes { get; }

        public string Parameters { get; }

        public ChannelModeMessage(Channel channel, string modes)
        {
            Channel = channel.Name;
            Modes = modes;
        }

        public ChannelModeMessage(string channelname, string modes)
        {
            Channel = channelname;
            Modes = modes;
        }

        public ChannelModeMessage(Channel channel, string modes, string parameters)
        {
            Channel = channel.Name;
            Modes = modes;
            Parameters = parameters;
        }

        public ChannelModeMessage(string channelname, string modes, string parameters)
        {
            Channel = channelname;
            Modes = modes;
            Parameters = parameters;
        }

        public void Send(StreamWriter writer, Client client)
        {
            if (string.IsNullOrWhiteSpace(Modes))
                writer.WriteLine($"MODE {Channel}");
            else if (string.IsNullOrWhiteSpace(Parameters))
                writer.WriteLine($"MODE {Channel} {Modes}");
            else
                writer.WriteLine($"MODE {Channel} {Modes} {Parameters}");
        }
    }
}