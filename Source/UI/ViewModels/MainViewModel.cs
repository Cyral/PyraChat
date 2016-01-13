using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pyratron.PyraChat.IRC;
using Pyratron.PyraChat.IRC.Messages.Send;
using Pyratron.PyraChat.UI.Models;

namespace Pyratron.PyraChat.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand EnterCommand { get; }
        public RelayCommand<UiChannel> SelectedChannelCommand { get; }

        public string ChatLineText
        {
            get { return chatLineText; }
            set
            {
                chatLineText = value;
                RaisePropertyChanged();
            }
        }

        public Network Network
        {
            get { return network; }
            set
            {
                network = value;
                RaisePropertyChanged();
            }
        }

        public UiChannel Channel
        {
            get { return channel; }
            set
            {
                channel = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Network> Networks
        {
            get { return networks; }
            set
            {
                networks = value;
                RaisePropertyChanged();
            }
        }

        private readonly Color[] colors =
        {
            Color.FromRgb(204, 62, 62),
            Color.FromRgb(204, 133, 62),
            Color.FromRgb(204, 204, 62),
            Color.FromRgb(62, 204, 62),
            Color.FromRgb(62, 204, 133),
            Color.FromRgb(62, 133, 204),
            Color.FromRgb(62, 204, 204),
            Color.FromRgb(204, 62, 204),
        };

        private readonly Random random = new Random();
        private UiChannel channel;
        private string chatLineText;

        private Network network;
        private ObservableCollection<Network> networks;

        public MainViewModel(bool designTime)
        {
            Networks = new ObservableCollection<Network>();
            EnterCommand = new RelayCommand(OnEnter);
            SelectedChannelCommand = new RelayCommand<UiChannel>(OnSelectedChannelChanged);
        }

        public void Connect()
        {
            Networks.Add(new Network("frogbox.es", 6667, new User("PyraChat", "PyraChat", "pyra"), new[]
            {
                //"#aenet",
               // "#Pyratron",
                "#Bricklayer",
                "#pyratest",
            }));
            /*
            Networks.Add(new Network("irc.esper.net", 6667, new User("Cyral33", "PyraChat", "pyrachat"), new[]
            {
                "#Codetree",
            }));
             */
            Networks.Add(new Network("irc.quakenet.org", 6667, new User("PyraChat", "PyraChat", "pyra"), new[]
            {
                "#pyratest",
            }));
           
            Network = Networks[0];
        }

        public Color GenColor()
        {
            return colors[random.Next(0, colors.Length)];
        }

        private void OnEnter()
        {
            var msg = new PrivateMessage(Channel.Name, ChatLineText);
            Channel.Send(msg);
            Channel.AddSelf(msg);
            ChatLineText = string.Empty;
        }

        private void OnSelectedChannelChanged(UiChannel newChannel)
        {
            Channel = newChannel;
            Channel.Unread = 0;
            Network = Channel.Network;
            Channel.Network.SortUsers();
        }
    }
}