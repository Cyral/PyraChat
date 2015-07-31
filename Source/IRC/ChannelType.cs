using System.Collections.Generic;
using System.Linq;

namespace Pyratron.PyraChat.IRC
{
    /// <summary>
    /// Channel type.
    /// </summary>
    /// <see cref="http://www.alien.net.au/irc/chantypes.html" />
    public sealed class ChannelType
    {
        public static readonly ChannelType Local = new ChannelType('&');
        public static readonly ChannelType Network = new ChannelType('#');
        public static readonly ChannelType Safe = new ChannelType('!');
        public static readonly ChannelType Unmoderated = new ChannelType('+');

        public char Prefix { get; }

        private static List<ChannelType> types;

        private ChannelType(char prefix)
        {
            if (types == null)
                types = new List<ChannelType>();
            Prefix = prefix;
            types.Add(this);
        }

        public override string ToString() => Prefix.ToString();

        public static ChannelType FromPrefix(char c)
        {
            var type = types.FirstOrDefault(t => t.Prefix.Equals(c));
            if (type != null)
                return type;
            throw new KeyNotFoundException($"Channel prefix \"{c}\" not found.");
        }
    }
}