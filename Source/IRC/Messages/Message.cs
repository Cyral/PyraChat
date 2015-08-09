using System;
using System.Linq;
using System.Text.RegularExpressions;
using Pyratron.PyraChat.IRC.Messages.Receive;
using Pyratron.PyraChat.IRC.Messages.Receive.Numerics;

namespace Pyratron.PyraChat.IRC.Messages
{
    public class Message
    {
        private static readonly Regex parseRegex = new Regex(@"(?:[:](\S+) )?(\S+)(?: (?!:)(.+?))?(?: [:](.+))?$");
        private static readonly char[] separator = {' '};
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

        /// <summary>
        /// Returns the user who sent the message. If null, a new user is created from the mask.
        /// </summary>
        public User User
            => Client.UserFromMask(Prefix) ?? new User(Prefix);

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
            Parameters =
                match.Groups[3].Value.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                    .Concat(new[] {match.Groups[4].Value})
                    .ToArray();
        }

        /// <summary>
        /// Finds a message type to handle the message.
        /// </summary>
        public ReceivableMessage Process()
        {
            // Named messages.
            if (NickMessage.CanProcess(this)) return new NickMessage(this);
            if (QuitMessage.CanProcess(this)) return new QuitMessage(this);
            if (JoinMessage.CanProcess(this)) return new JoinMessage(this);
            if (PartMessage.CanProcess(this)) return new PartMessage(this);
            if (PrivateMessage.CanProcess(this)) return new PrivateMessage(this);
            if (PingMessage.CanProcess(this)) return new PingMessage(this);
            if (NoticeMessage.CanProcess(this)) return new NoticeMessage(this);
            if (UserModeMessage.CanProcess(this)) return new UserModeMessage(this);
            if (ChannelModeMessage.CanProcess(this)) return new ChannelModeMessage(this);
            if (KickMessage.CanProcess(this)) return new KickMessage(this);
            if (InviteMessage.CanProcess(this)) return new InviteMessage(this);
            if (OperwallMessage.CanProcess(this)) return new OperwallMessage(this);

            // Numerics.
            if (NumericMessage.CanProcess(this))
            {
                // Pass all numeric messages to NumericMessage so an event can be fired, then pass it to more specific instances.
                // ReSharper disable once ObjectCreationAsStatement
                new NumericMessage(this);

                if (WelcomeMessage.CanProcess(this)) return new WelcomeMessage(this);
                if (YourHostMessage.CanProcess(this)) return new YourHostMessage(this);
                if (CreatedMessage.CanProcess(this)) return new CreatedMessage(this);
                if (MyInfoMessage.CanProcess(this)) return new MyInfoMessage(this);
                if (SupportMessage.CanProcess(this)) return new SupportMessage(this);
                if (BounceMessage.CanProcess(this)) return new BounceMessage(this);
                if (MOTDEndMessage.CanProcess(this)) return new MOTDEndMessage(this);
                if (MOTDStartMessage.CanProcess(this)) return new MOTDStartMessage(this);
                if (MOTDMessage.CanProcess(this)) return new MOTDMessage(this);
                if (LUserMessage.CanProcess(this)) return new LUserMessage(this);
                if (NamesMessage.CanProcess(this)) return new NamesMessage(this);
                if (EndOfNamesMessage.CanProcess(this)) return new EndOfNamesMessage(this);
                if (TopicMessage.CanProcess(this)) return new TopicMessage(this);
                if (TopicWhoTimeMessage.CanProcess(this)) return new TopicWhoTimeMessage(this);
                if (ListMessage.CanProcess(this)) return new ListMessage(this);
                if (ListEndMessage.CanProcess(this)) return new ListEndMessage(this);
                if (YoureOperMessage.CanProcess(this)) return new YoureOperMessage(this);
                if (AwayMessage.CanProcess(this)) return new AwayMessage(this);
                if (UnAwayMessage.CanProcess(this)) return new UnAwayMessage(this);
                if (NowAwayMessage.CanProcess(this)) return new NowAwayMessage(this);
                if (UModeIsMessage.CanProcess(this)) return new UModeIsMessage(this);
                if (VersionMessage.CanProcess(this)) return new VersionMessage(this);
                if (TimeMessage.CanProcess(this)) return new TimeMessage(this);
                if (WhoMessage.CanProcess(this)) return new WhoMessage(this);
                if (WhoisMessage.CanProcess(this)) return new WhoisMessage(this);
                if (EndOfWhoMessage.CanProcess(this)) return new EndOfWhoMessage(this);
                if (EndOfWhoisMessage.CanProcess(this)) return new EndOfWhoisMessage(this);
                if (BanListMessage.CanProcess(this)) return new BanListMessage(this);
                if (EndOfBanListMessage.CanProcess(this)) return new EndOfBanListMessage(this);
                if (InviteListMessage.CanProcess(this)) return new InviteListMessage(this);
                if (EndOfInviteListMessage.CanProcess(this)) return new EndOfInviteListMessage(this);
                if (ExceptListMessage.CanProcess(this)) return new ExceptListMessage(this);
                if (EndOfExceptListMessage.CanProcess(this)) return new EndOfExceptListMessage(this);
                if (IsOnMessage.CanProcess(this)) return new IsOnMessage(this);

                // Catch all for unhandled error messages.
                if (ErrorMessage.CanProcess(this)) return new ErrorMessage(this);
            }

            Console.WriteLine("Message handler for \"" + Text + "\" not found.");
            return null;
        }
    }
}