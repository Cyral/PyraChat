namespace Pyratron.PyraChat.IRC
{
    public class User
    {
        public string Nick { get; private set; }
        public string RealName { get; private set; }
        public string Ident { get; private set; }
        public string Host { get; private set; }
        public string Mode { get; private set; }

        public User(Client client, string nick)
        {
            Nick = nick;
        }
    }
}