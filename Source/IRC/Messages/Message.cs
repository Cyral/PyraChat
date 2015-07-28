using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Pyratron.PyraChat.IRC.Messages.Receive;
using Pyratron.PyraChat.IRC.Messages.Receive.Numerics;

namespace Pyratron.PyraChat.IRC.Messages
{
    public class Message
    {
        private static readonly Regex parseRegex = new Regex(@"^(?:[:](\S+) )?(\S+)(?: (?!:)(.+?))?(?: [:](.+))?$");

        private static readonly Dictionary<Predicate<Message>, Type> processors =
            new Dictionary<Predicate<Message>, Type>();

        public Client Client { get; }

        /// <summary>
        /// Full message text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Message prefix.
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        /// Message destination channel or user. (Ex: #Test)
        /// </summary>
        public string Destination => Parameters[0];

        /// <summary>
        /// A string representation of the complete parameters array.
        /// </summary>
        public string ParamsString => string.Join(" ", Parameters);

        /// <summary>
        /// Message type. (Ex: PRIVMSG)
        /// </summary>
        public string Type { get; }

        public Channel Channel
            => Client.Channels.FirstOrDefault(c => c.Name.Equals(Destination, StringComparison.OrdinalIgnoreCase));

        public bool IsChannel => Channel != null;

        public User User
            => Client.User; //TODO: Get user from mask

        /// <summary>
        /// Message parameters.
        /// </summary>
        public string[] Parameters { get; }

        static Message()
        {
            //Add message types
            processors.Add(ChannelNoticeMessage.CanProcess, typeof (ChannelNoticeMessage));
            processors.Add(UserNoticeMessage.CanProcess, typeof (UserNoticeMessage));
            processors.Add(WelcomeMessage.CanProcess, typeof(WelcomeMessage));
            processors.Add(PingMessage.CanProcess, typeof(PingMessage));
        }

        public Message(Client client, string message)
        {
            Client = client;
            Text = message;

            var match = parseRegex.Match(message);
            if (!match.Success) return;

            Prefix = match.Groups[1].Value;
            Type = match.Groups[2].Value;
            Parameters = match.Groups[3].Value.Split(' ').Concat(new[] {match.Groups[4].Value}).ToArray();
        }

        /// <summary>
        /// Finds a message type to handle the message.
        /// </summary>
        public void Process()
        {
            foreach (var processor in processors)
            {
                if (processor.Key.Invoke(this) && processor.Value.GetInterfaces().Contains(typeof(IReceivable)))
                {
                    var receivable = Activator.CreateInstance(processor.Value) as IReceivable;
                    receivable?.Process(this);
                    break;
                }
            }
        }
    }
}