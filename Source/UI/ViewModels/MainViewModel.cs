using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
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

        private Network network;
        private UiChannel channel;
        private string chatLineText;
        private ObservableCollection<Network> networks;

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

        public MainViewModel(bool designTime)
        {
            Networks = new ObservableCollection<Network>();
            EnterCommand = new RelayCommand(OnEnter);
            SelectedChannelCommand = new RelayCommand<UiChannel>(param => OnSelectedChannelChanged(param));
        }

        public void Connect()
        {
            var user = new User("PyraChat", "PyraChat", "pyra");
            Networks.Add(new Network("frogbox.es", 6667, user, new[]
            {
                    //"#aenet",
                    //"#Pyratron",
                    "#Bricklayer",
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

        private void OnSelectedChannelChanged(UiChannel channel)
        {
            Channel = channel;
            Channel.Network.SortUsers();
        }
    }
}