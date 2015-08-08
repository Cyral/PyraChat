using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_TOPICWHOTIME message. (333)
    /// </summary>
    /// <remarks>
    /// Not supported on all servers. (not RFC, ircu)
    /// </remarks>
    public class TopicWhoTimeMessage : ReceivableMessage
    {
        public User Author { get; }
        public DateTime Date { get; }

        public Channel Channel
            =>
                BaseMessage.Client.Channels.FirstOrDefault(
                    c => c.Name.Equals(BaseMessage.Parameters[1], StringComparison.OrdinalIgnoreCase));

        public TopicWhoTimeMessage(Message msg) : base(msg)
        {
            Author = new User(msg.Parameters[2]);
            Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            Date = Date.AddSeconds(int.Parse(msg.Parameters[3])).ToLocalTime();
            Channel.Topic.Date = Date;
            Channel.Topic.Author = Author;
            Channel.OnTopicWhoTime(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "333";
        }
    }
}