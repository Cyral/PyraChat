namespace Pyratron.PyraChat.IRC
{
    public class User
    {
        /// <summary>
        /// Current nickname.
        /// </summary>
        public string Nick { get; private set; }

        /// <summary>
        /// Real name.
        /// </summary>
        public string RealName { get; private set; }

        /// <summary>
        /// Ident (username).
        /// </summary>
        public string Ident { get; private set; }
        public string Host { get; private set; }
        public string Mode { get; private set; }

        public User(string nick, string realName, string ident)
        {
            Nick = nick;
            RealName = realName;
            Ident = ident;
        }

        public User(Client client, string nick)
        {
            Nick = nick;
        }
    }
}