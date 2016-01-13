using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using Pyratron.PyraChat.IRC;
using Pyratron.PyraChat.IRC.Messages.Receive;
using Pyratron.PyraChat.UI.ViewModels;
using TopicMessage = Pyratron.PyraChat.IRC.Messages.Receive.Numerics.TopicMessage;

namespace Pyratron.PyraChat.UI.Models
{
    public class UiChannel : ObservableObject
    {
        /// <summary>
        /// Underlying channel.
        /// </summary>
        public Channel Channel { get; set; }

        /// <summary>
        /// Chat lines.
        /// </summary>
        public ObservableCollection<ChatLine> Lines
        {
            get { return lines; }
            set
            {
                lines = value;
                RaisePropertyChanged();
            }
        }

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

        public int Unread
        {
            get { return unread; }
            set
            {
                unread = value;
                UnreadString = unread > 0 ? unread.ToString() : string.Empty;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(UnreadString));
            }
        }

        public string Modes
        {
            get
            {
                if (modes.Length > 0)
                    return "+" + modes;
                return string.Empty;
            }
            set
            {
                modes = value;
                RaisePropertyChanged();
            }
        }

        public string UnreadString { get; private set; }

        internal Network Network { get; }

        private ObservableCollection<ChatLine> lines;

        private string topic, name, modes;
        private int unread;

        public UiChannel(Channel channel, Network network)
        {
            Channel = channel;
            Network = network;
            channel.TopicChange += ChannelOnTopicChange;
            channel.ModeAdd += ChannelOnModeAdd;
            channel.ModeRemove += ChannelOnModeRemove;
            Name = channel.Name;
            Lines = new ObservableCollection<ChatLine>();
        }

        public void AddLine(PrivateMessage privateMessage)
        {
            var user = Network.GetUser(privateMessage.BaseMessage.User);
            Lines.Add(new ChatLine(user, privateMessage.Message));
            if (ViewModelLocator.Main.Channel != this)
                Unread++;
        }

        public void AddSelf(IRC.Messages.Send.PrivateMessage privateMessage)
        {
            Lines.Add(new ChatLine(Network.Me, privateMessage.Message));
        }

        public void AddSystemLine(string text, Color color)
        {
            Lines.Add(new ChatLine(text, color));
        }

        public void Send(IRC.Messages.Send.PrivateMessage msg)
        {
            Network.Client.Send(msg);
        }

        private void ChannelOnModeAdd(char mode)
        {
            UpdateModeString();
        }

        private void ChannelOnModeRemove(char mode)
        {
            UpdateModeString();
        }

        private void ChannelOnTopicChange(TopicMessage message)
        {
            Topic = message.Topic;
        }

        private void UpdateModeString()
        {
            Modes = string.Join(string.Empty, Channel.Modes.OrderBy(char.IsLower).ThenBy(x=>x));
        }
    }
}