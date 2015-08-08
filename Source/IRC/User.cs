using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pyratron.PyraChat.IRC
{
    public class User
    {
        /// <summary>
        /// Current nickname.
        /// </summary>
        public string Nick { get; internal set; }

        /// <summary>
        /// Real name.
        /// </summary>
        public string RealName { get; private set; }

        /// <summary>
        /// Ident (username).
        /// </summary>
        public string Ident { get; internal set; }

        public string Host { get; internal set; }
        public List<char> Modes { get; private set; } = new List<char>();
        public List<Channel> Channels { get; internal set; } = new List<Channel>();
        public UserRank Rank { get; set; } = UserRank.None;

        public bool IsAway => Modes.Contains('a');
        public bool IsInvisible => Modes.Contains('i');
        public bool IsRestricted => Modes.Contains('r');
        public bool IsOperator => Modes.Contains('o');

        internal static Regex MaskRegex { get; } =
            new Regex(@"([a-z0-9_\-\[\]\\`|^{}]+)!([a-z0-9_\-\~]+)\@([a-z0-9\.\-]+)", RegexOptions.IgnoreCase);

        public User(string mask)
        {
            var match = MaskRegex.Match(mask);
            if (!match.Success) return;
            Nick = match.Groups[1].Value;
            Ident = match.Groups[2].Value;
            Host = match.Groups[3].Value;
        }

        public User(string nick, string realname, string ident, string hostname = "")
        {
            Nick = nick;
            RealName = realname;
            Ident = ident;
            Host = hostname;
        }

        public User(string nick, UserRank rank)
        {
            Nick = nick;
            Rank = rank;
        }

        public void AddMode(Client client, char mode)
        {
            if (!Modes.Contains(mode))
                Modes.Add(mode);

            // Invoke events
            if (mode.Equals('a'))
                client.OnAwayChange(this, true);
        }

        public void RemoveMode(Client client, char mode)
        {
            if (Modes.Contains(mode))
                Modes.Remove(mode);

            // Invoke events
            if (mode.Equals('a'))
                client.OnAwayChange(this, false);
        }
    }
}