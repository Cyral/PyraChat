using System.Text.RegularExpressions;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_BOUNCE message. (005)
    /// </summary>
    public class BounceMessage : ReceivableMessage
    {
        private static readonly Regex regex = new Regex(@"Try server ([a-z0-9-.]+), port ([0-9]+)",
            RegexOptions.IgnoreCase);

        /// <summary>
        /// Suggested host to connect to.
        /// </summary>
        public string Host { get; }

        public string Port { get; }

        public BounceMessage(Message msg) : base(msg)
        {
            var match = regex.Match(msg.Parameters[1]);
            if (match.Success)
            {
                Host = match.Groups[1].Value;
                Port = match.Groups[2].Value;
                msg.Client.OnReplyBounce(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            //"Try server" differentiates between RPL_ISUPPORT method with the same ID.
            //See https://www.alien.net.au/irc/irc2numerics.html
            return msg.Type == "005" && msg.Parameters[1].StartsWith("Try server");
        }
    }
}