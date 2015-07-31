using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text.RegularExpressions;
using Pyratron.PyraChat.IRC.Messages.Receive;
using Pyratron.PyraChat.IRC.Messages.Receive.Numerics;

namespace Pyratron.PyraChat.IRC.Messages
{
    public class Message
    {
        private static readonly Regex parseRegex = new Regex(@"(?:[:](\S+) )?(\S+)(?: (?!:)(.+?))?(?: [:](.+))?$");

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
        public ReceivableMessage Process()
        {
            if (ChannelNoticeMessage.CanProcess(this)) return new ChannelNoticeMessage(this);
            if (JoinMessage.CanProcess(this)) return new JoinMessage(this);
            if (PrivateMessage.CanProcess(this)) return new PrivateMessage(this);
            if (PingMessage.CanProcess(this)) return new PingMessage(this);
            if (UserNoticeMessage.CanProcess(this)) return new UserNoticeMessage(this);

            if (WelcomeMessage.CanProcess(this)) return new WelcomeMessage(this);
            if (YourHostMessage.CanProcess(this)) return new YourHostMessage(this);
            if (CreatedMessage.CanProcess(this)) return new CreatedMessage(this);
            if (MyInfoMessage.CanProcess(this)) return new MyInfoMessage(this);
            if (SupportMessage.CanProcess(this)) return new SupportMessage(this);
            if (BounceMessage.CanProcess(this)) return new BounceMessage(this);
            if (MOTDEndMessage.CanProcess(this)) return new MOTDEndMessage(this);
            if (MOTDStartMessage.CanProcess(this)) return new MOTDStartMessage(this);
            if (MOTDMessage.CanProcess(this)) return new MOTDMessage(this);

            Console.WriteLine("Message handler for \"" + Text + "\" not found.");
            return null;
        }
    }
}