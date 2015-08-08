using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Topic message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.4" />
    public class TopicMessage : SendableMessage
    {
        public string Channel { get; }
        public string Topic { get; }

        /// <summary>
        /// Requests the topic of the channel.
        /// </summary>
        public TopicMessage(string channel)
        {
            Channel = channel;
        }

        /// <summary>
        /// Requests the topic of the channel.
        /// </summary>
        public TopicMessage(Channel channel)
        {
            Channel = channel.Name;
        }

        /// <summary>
        /// Sets the topic of the channel.
        /// </summary>
        public TopicMessage(string channel, string topic)
        {
            Channel = channel;
            Topic = topic;
        }

        /// <summary>
        /// Sets the topic of the channel.
        /// </summary>
        public TopicMessage(Channel channel, string topic)
        {
            Channel = channel.Name;
            Topic = topic;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine(Topic != null ? $"TOPIC {Channel}" : $"TOPIC {Channel} :{Topic}");
        }
    }
}