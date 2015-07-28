using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Pyratron.PyraChat.IRC;

namespace Pyratron.PyraChat.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                RaisePropertyChanged();
            }
        }

        private IRCClient irc;

        public MainViewModel()
        { 
            irc = new IRCClient("frogbox.es", 6667);
            irc.MessageReceived += message => Text += message + Environment.NewLine;
            irc.Connect();
        }
    }
}
