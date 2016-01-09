using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Pyratron.PyraChat.IRC;
using Pyratron.PyraChat.IRC.Messages.Receive.Numerics;

namespace Pyratron.PyraChat.UI.Models
{
    public class UiChannel : ObservableObject
    {
        public Channel Channel { get; set; }

        public List<string> Buffer { get; set; }

        public string Topic
        {
            get { return topic; }
            set
            {
                topic = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private string topic, name;

        public UiChannel(Channel channel)
        {
            Channel = channel;
            channel.TopicChange += ChannelOnTopicChange;
            Name = channel.Name;
            Buffer = new List<string>();
        }

        private void ChannelOnTopicChange(TopicMessage message)
        {
            Topic = message.Topic;
        }
    }
}