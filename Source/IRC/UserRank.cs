using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyratron.PyraChat.IRC
{
    /// <summary>
    /// User's rank.
    /// </summary>
    public sealed class UserRank
    {
        public static readonly UserRank None = new UserRank('\0');
        public static readonly UserRank Voice = new UserRank('+');
        public static readonly UserRank HalfOp = new UserRank('%');
        public static readonly UserRank Op = new UserRank('@');
        public static readonly UserRank Creator = new UserRank('@');

        public char Prefix { get; }

        private static readonly List<UserRank> types = new List<UserRank>();

        private UserRank(char prefix)
        {
            Prefix = prefix;
            types.Add(this);
        }

        public override string ToString() => Prefix.ToString();

        public static UserRank FromPrefix(char c)
        {
            var type = types.FirstOrDefault(t => t.Prefix.Equals(c));
            return type ?? None;
        }
    }
}
