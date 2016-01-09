using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Pyratron.PyraChat.IRC;
using Pyratron.PyraChat.UI.ViewModels;
using TopicMessage = Pyratron.PyraChat.IRC.Messages.Receive.Numerics.TopicMessage;

namespace Pyratron.PyraChat.UI.Models
{
    public class UiChannel : ObservableObject
    {
        public Channel Channel { get; set; }


        public ObservableCollection<ChatLine> Lines
        {
            get { return lines; }
            set
            {
                lines = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ChatLine> lines;

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
            Lines = new ObservableCollection<ChatLine>();
        }

        public void AddLine(IRC.Messages.Receive.PrivateMessage privateMessage)
        {
            var user = ViewModelLocator.Main.GetUser(privateMessage.BaseMessage.User);
            Lines.Add(new ChatLine(user, privateMessage.Message));
        }

        public void AddLine(IRC.Messages.Send.PrivateMessage privateMessage, UiUser user)
        {
            Lines.Add(new ChatLine(user, privateMessage.Message));
        }

        private void ChannelOnTopicChange(TopicMessage message)
        {
            Topic = message.Topic;
        }
    }
}