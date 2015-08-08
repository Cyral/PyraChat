using System;

namespace Pyratron.PyraChat.IRC.Messages
{
    /// <summary>
    /// Represents a channel topic.
    /// </summary>
    public class ChannelTopic
    {
        /// <summary>
        /// Topic text.
        /// </summary>
        public string Message { get; internal set; }

        /// <summary>
        /// Topic author. (Available on servers supporting RPL_TOPICWHOTIME)
        /// </summary>
        public User Author { get; internal set; }

        /// <summary>
        /// Topic set date. (Available on servers supporting RPL_TOPICWHOTIME)
        /// </summary>
        public DateTime Date { get; internal set; }

        public override string ToString()
        {
            return Author != null
                ? $"\"{Message}\" set by {Author.Nick} on {Date}"
                : Message;
        }

        public void Reset()
        {
            Date = DateTime.Now;
            Author = null;
            Message = string.Empty;
        }
    }
}