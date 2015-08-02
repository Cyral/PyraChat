using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyratron.PyraChat.UI.Models
{
    /// <summary>
    /// Represents settings for an IRC server.
    /// </summary>
    public class ServerSettings
    {
        /// <summary>
        /// Hostname.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Port.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Automatically connect at startup or when disconnected.
        /// </summary>
        public bool AutoConnect { get; }

        /// <summary>
        /// Use an SSL connection.
        /// </summary>
        public bool UseSSL { get; }

        /// <summary>
        /// Accept invalid SSL certificates.
        /// </summary>
        public bool AcceptInvalidSSLCertificates { get; }
    }
}
