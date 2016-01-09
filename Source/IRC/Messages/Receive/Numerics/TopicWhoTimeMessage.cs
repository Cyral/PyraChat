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
            Author = msg.Client.UserFromMask(msg.Parameters[2]);
            Date = Utils.UnixTimeStampToDateTime(int.Parse(msg.Parameters[3]));
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