using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_WHOREPLY message. (352)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.6.1" />
    public class WhoMessage : ReceivableMessage
    {
        public string Channel => BaseMessage.Parameters[1];
        public string Ident => BaseMessage.Parameters[2];
        public string Host => BaseMessage.Parameters[3];
        public string Server => BaseMessage.Parameters[4];
        public string Nick => BaseMessage.Parameters[5];
        public string Realname { get; }

        /// <summary>
        /// The user who's information matches the WHO message. Null if user is not recognized.
        /// </summary>
        public User User { get; }

        public WhoMessage(Message msg) : base(msg)
        {
            var info = BaseMessage.Parameters[BaseMessage.Parameters.Length - 1].Split(' ');
            if (info.Length > 1)
                Realname = string.Join(" ", info.Skip(1)).Trim();
            var user = msg.Client.UserFromNick(Nick);
            if (user != null) //If the user is recognized (Because of a previous JOIN message), fill in their details.
            {
                user.Host = Host;
                user.RealName = Realname;
                user.Ident = Ident;

                var infoParams = msg.Parameters[6];

                foreach (var character in infoParams)
                {
                    switch (character)
                    {
                        case '*':
                            user.IsOp = true;
                            break;
                        case 'G': //Handle gone/here
                            user.SetIsAway(msg.Client, true);
                            break;
                        case 'H':
                            user.SetIsAway(msg.Client, false);
                            break;
                        default: //Handle ranks
                            var rank = UserRank.FromPrefix(character);
                            if (rank != UserRank.None)
                                user.AddRank(msg.Client, rank);
                            break;
                    }
                }
            }
            msg.Client.OnReplyWho(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "352";
        }
    }
}