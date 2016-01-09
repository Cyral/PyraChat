using System.Windows.Media;
using GalaSoft.MvvmLight;
using Pyratron.PyraChat.IRC;

namespace Pyratron.PyraChat.UI.Models
{
    public class UiUser : ObservableObject
    {
        private UserRank rank;
        public Color Color { get; set; }

        public User User { get; set; }

        /// <summary>
        /// Rank for the current channel being displayed.
        /// </summary>
        public UserRank Rank
        {
            get { return rank; }
            set
            {
                rank = value; 
                RaisePropertyChanged();
            }
        }

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