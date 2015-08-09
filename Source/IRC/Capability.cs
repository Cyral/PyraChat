using System;
using System.Collections.Generic;
using System.Linq;

namespace Pyratron.PyraChat.IRC
{
    /// <summary>
    /// IRCv3 Capabilities
    /// </summary>
    /// <see cref="http://ircv3.net/irc/"/>
    public class Capability
    {
        //Supported capabilities
        public static readonly Capability AwayNotify = new Capability("away-notify");
        public static readonly Capability MultiPrefix = new Capability("multi-prefix");

        //Capabilities unsupported by PyraChat
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability SASL = new Capability("sasl");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability AccountNotify = new Capability("account-notify");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability ExtendedJoin = new Capability("extended-join");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability TLS = new Capability("tls");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability Metadata= new Capability("account-notify");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability Monitor = new Capability("monitor");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability AccountTag = new Capability("account-tag");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability Batch = new Capability("batch");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability CapNotify = new Capability("cap-notify");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability ChangeHost = new Capability("chghost");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability EchoMessage = new Capability("echo-message");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability InviteNotify = new Capability("invite-notify");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability ServerTime = new Capability("server-time");
        [Obsolete("Unsupported/Not Implemented")]
        public static readonly Capability UserhostInNames = new Capability("userhost-in-names");

        private static List<Capability> types;
        public string Name { get; }

        private Capability(string name)
        {
            if (types == null)
                types = new List<Capability>();
            Name = name;
            types.Add(this);
        }

        public override string ToString() => Name;

        public static Capability FromName(string capability)
        {
            var type = types.FirstOrDefault(t => t.Name.Equals(capability, StringComparison.OrdinalIgnoreCase));
            return type;
        }
    }
}