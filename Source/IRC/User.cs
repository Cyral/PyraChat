using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        public string RealName { get; internal set; }

        /// <summary>
        /// Ident (username).
        /// </summary>
        public string Ident { get; internal set; }

        public string Host { get; internal set; }
        public List<char> Modes { get; } = new List<char>();
        public List<Channel> Channels { get; internal protected set; } = new List<Channel>();
        public bool IsAway => Modes.Contains('a') || isAway;
        public bool IsInvisible => Modes.Contains('i');
        public bool IsRestricted => Modes.Contains('r');
        public bool IsOperator => Modes.Contains('o') || IsOp;

        public UserRank Rank => GetRank("#pyratest");

        public string AwayMessage
        {
            get { return string.IsNullOrWhiteSpace(awayMessage) ? "Away" : awayMessage; }
            private set { awayMessage = value; }
        }

        internal static Regex MaskRegex { get; } =
            new Regex(@"([a-z0-9_\-\[\]\\`|^{}]+)!([a-z0-9_\-\~]+)\@([a-z0-9\.\-]+)", RegexOptions.IgnoreCase);

        private readonly ConcurrentDictionary<string, List<UserRank>> ranks = new ConcurrentDictionary<string, List<UserRank>>();
        private bool isAway;
        internal bool IsOp;
        private string awayMessage;

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

        public User(string nick, string channel, UserRank rank)
        {
            Nick = nick;
            if (!ranks.ContainsKey(channel))
                ranks.TryAdd(channel, new List<UserRank>());
            ranks[channel].Add(rank);
        }

        public void AddRank(Client client, string channel, UserRank rank)
        {
            if (!ranks.ContainsKey(channel))
                ranks.TryAdd(channel, new List<UserRank>());
            if (!ranks[channel].Contains(rank))
            {
                if (!ranks[channel].Contains(UserRank.None))
                    ranks[channel].Add(UserRank.None);
                ranks[channel].Add(rank);
                client.OnRankChange(this, channel, ranks[channel].Min());
            }
        }

        public void RemoveRank(Client client, string channel, UserRank rank)
        {
            if (ranks.ContainsKey(channel) && ranks[channel].Contains(rank))
            {
                ranks[channel].Remove(rank);
                if (!ranks[channel].Contains(UserRank.None))
                    ranks[channel].Add(UserRank.None);
                client.OnRankChange(this, channel, ranks[channel].Min());
            }
        }

        public UserRank GetRank(Channel channel)
        {
            return GetRank(channel.Name);
        }

        public UserRank GetRank(string channel)
        {
            if (!ranks.ContainsKey(channel))
            {
                ranks.TryAdd(channel, new List<UserRank>());
                ranks[channel].Add(UserRank.None);
            }
            return ranks[channel].Min();
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

        internal void SetIsAway(Client client, bool away, string reason = "")
        {
            AwayMessage = away ? reason : string.Empty;
            isAway = away;
            client.OnAwayChange(this, away);
        }
    }
}