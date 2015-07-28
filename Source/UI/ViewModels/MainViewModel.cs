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

        private Client irc;

        public MainViewModel()
        { 
            irc = new Client("frogbox.es", 6667, new User("My_Name", "My Real Name", "USFF0000"));
            irc.MessageReceived += message => Text += message + Environment.NewLine;
            irc.Connect += client => Text += "Connected!";
            //irc.Notice += (client, user, notice) => Text += notice + Environment.NewLine;
            irc.Start();
        }
    }
}
