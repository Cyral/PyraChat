using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// RPL_TOPIC and RPL_NOTOPIC messages. (332 and 331)
    /// </summary>
    public class TopicMessage : Numerics.TopicMessage
    {
        public override sealed string Topic => BaseMessage.Parameters[1];

        public override sealed Channel Channel =>
            BaseMessage.Client.Channels.FirstOrDefault(
                c => c.Name.Equals(BaseMessage.Parameters[0], StringComparison.OrdinalIgnoreCase));

        public TopicMessage(Message msg) : base(msg)
        {
            Channel.Topic.Reset();
            Channel.Topic.Message = Topic;
            Channel.OnTopicChange(this);
        }

        public new static bool CanProcess(Message msg)
        {
            return msg.Type == "TOPIC";
        }
    }
}