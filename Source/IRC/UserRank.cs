using System.Collections.Generic;
using System.Linq;

namespace Pyratron.PyraChat.IRC
{
    /// <summary>
    /// User's rank.
    /// </summary>
    public sealed class UserRank
    {
        public static readonly UserRank None = new UserRank();
        public static readonly UserRank Voice = new UserRank('+');
        public static readonly UserRank HalfOp = new UserRank('%');
        public static readonly UserRank Op = new UserRank('@');
        public static readonly UserRank Admin = new UserRank('&');
        public static readonly UserRank Owner = new UserRank('~');
        private static List<UserRank> types;
        public char Prefix { get; }

        private UserRank()
        {
            if (types == null)
                types = new List<UserRank>();
            types.Add(this);
        }

        private UserRank(char prefix)
        {
            Prefix = prefix;
        }

        public override string ToString() => Prefix.ToString();

        public static UserRank FromPrefix(char c)
        {
            var type = types.FirstOrDefault(t => t.Prefix.Equals(c));
            return type ?? None;
        }
    }
}