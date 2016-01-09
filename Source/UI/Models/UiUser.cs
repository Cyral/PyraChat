using System.Windows.Media;
using GalaSoft.MvvmLight;
using Pyratron.PyraChat.IRC;

namespace Pyratron.PyraChat.UI.Models
{
    public class UiUser : ObservableObject
    {
        public Color Color { get; set; }

        public User User { get; set; }

        public UiUser(User user)
        {
            User = user;
            Color = Colors.Black;
        }

        public UiUser(User user, Color color)
        {
            User = user;
            Color = color;
        }
    }
}