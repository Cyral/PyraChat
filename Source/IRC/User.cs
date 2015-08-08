using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Pyratron.PyraChat.IRC.Messages.Receive.Numerics;

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
        public UserRank Rank => ranks.Min();
        private List<UserRank> ranks = new List<UserRank>(); 

        public bool IsAway => Modes.Contains('a') || isAway;
        public bool IsInvisible => Modes.Contains('i');
        public bool IsRestricted => Modes.Contains('r');
        public bool IsOperator => Modes.Contains('o');
        public string AwayMessage { get; private set; }

        private bool isAway;

        internal static Regex MaskRegex { get; } =
            new Regex(@"([a-z0-9_\-\[\]\\`|^{}]+)!([a-z0-9_\-\~]+)\@([a-z0-9\.\-]+)", RegexOptions.IgnoreCase);

        public User(string mask)
        {
            ranks.Add(UserRank.None);
            var match = MaskRegex.Match(mask);
            if (!match.Success) return;
            Nick = match.Groups[1].Value;
            Ident = match.Groups[2].Value;
            Host = match.Groups[3].Value;
        }

        public User(string nick, string realname, string ident, string hostname = "")
        {
            ranks.Add(UserRank.None);
            Nick = nick;
            RealName = realname;
            Ident = ident;
            Host = hostname;
        }

        public User(string nick, UserRank rank)
        {
            ranks.Add(UserRank.None);
            Nick = nick;
            ranks.Add(rank);
        }

        public void AddRank(Client client, UserRank rank)
        {
            if (!ranks.Contains(rank))
            {
                ranks.Add(rank);
                client.OnRankChange(this, Rank);
            }
        }

        public void RemoveRank(Client client, UserRank rank)
        {
            if (ranks.Contains(rank))
            {
                ranks.Remove(rank);
                client.OnRankChange(this, Rank);
            }
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

        internal void SetIsAway(Client client, bool away, string reason ="")
        {
            AwayMessage = away ? reason : string.Empty;
            isAway = away;
            client.OnAwayChange(this, away);
        }
    }
}