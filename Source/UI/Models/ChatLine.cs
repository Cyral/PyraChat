using System;
using System.Windows.Media;

namespace Pyratron.PyraChat.UI.Models
{
    /// <summary>
    /// Represents a line of the chat.
    /// </summary>
    public class ChatLine
    {
        /// <summary>
        /// User who sent the message. Null if system message.
        /// </summary>
        public UiUser Sender { get; }

        /// <summary>
        /// Message text.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Time message was received.
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// Indicates if this is a sytem message with no associated user.
        /// </summary>
        public bool System { get; }

        /// <summary>
        /// The color of the message, if it is a system message.
        /// </summary>
        public Color Color { get; } = Globals.SystemColor;

        /// <summary>
        /// Creates a new chat line from the specified user.
        /// </summary>
        public ChatLine(UiUser user, string message)
        {
            Sender = user;
            Message = message.TrimEnd(' ');
            Time = DateTime.Now;
        }

        /// <summary>
        /// Creates a system chat line.
        /// </summary>
        public ChatLine(string message)
        {
            System = true;
            Message = message.TrimEnd(' ');
            Time = DateTime.Now;
        }

        /// <summary>
        /// Creates a system chat line.
        /// </summary>
        public ChatLine(string message, Color color) : this(message)
        {
            Color = color;
        }
    }
}