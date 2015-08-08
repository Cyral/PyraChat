using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_TOPIC and RPL_NOTOPIC messages. (332 and 331)
    /// </summary>
    public class TopicMessage : ReceivableMessage
    {
        public string Topic => BaseMessage.Parameters[2];

        public Channel Channel =>
            BaseMessage.Client.Channels.FirstOrDefault(
                c => c.Name.Equals(BaseMessage.Parameters[1], StringComparison.OrdinalIgnoreCase));

        public TopicMessage(Message msg) : base(msg)
        {
            Channel.Topic.Reset();
            Channel.Topic.Message = Topic;
            Channel.OnTopicChange(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "332" || msg.Type == "331";
        }
    }
}