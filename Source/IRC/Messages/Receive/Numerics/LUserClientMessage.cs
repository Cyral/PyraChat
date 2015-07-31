using System.Text.RegularExpressions;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_LUSERCLIENT message. (251)
    /// </summary>
    public class LUserClientMessage : ReceivableMessage
    {
        private static readonly Regex regex = new Regex(@"There are ([0-9]+) users and ([0-9]+) invisible on ([0-9]+) servers",
            RegexOptions.IgnoreCase);

        /// <summary>
        /// Number of visible users online the server at the time of this message.
        /// </summary>
        public int VisibleUserCount { get; }

        /// <summary>
        /// Number of visible users online the server at the time of this message.
        /// </summary>
        public int InvisibleUserCount { get; }

        /// <summary>
        /// Number of visible users online the server at the time of this message.
        /// </summary>
        public int ServerCount { get; }

        public LUserClientMessage(Message msg) : base(msg)
        {
            var match = regex.Match(msg.Parameters[1]);
            if (match.Success)
            {
                int visibleUserCount, invisibleUserCount, serverCount;
                int.TryParse(match.Groups[1].Value, out visibleUserCount);
                int.TryParse(match.Groups[2].Value, out invisibleUserCount);
                int.TryParse(match.Groups[3].Value, out serverCount);

                VisibleUserCount = visibleUserCount;
                InvisibleUserCount = invisibleUserCount;
                ServerCount = serverCount;

                msg.Client.OnReplyLUserClient(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "251";
        }
    }
}