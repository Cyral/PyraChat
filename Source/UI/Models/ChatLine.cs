using System;

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

        public bool System { get; }

        /// <summary>
        /// Creates a new chat line from the specified user.
        /// </summary>
        public ChatLine(UiUser user, string message)
        {
            Sender = user;
            Message = message;
            Time = DateTime.Now;
        }

        /// <summary>
        /// Creates a system chat line.
        /// </summary>
        public ChatLine(string message)
        {
            System = true;
            Message = message;
            Time = DateTime.Now;
        }
    }
}