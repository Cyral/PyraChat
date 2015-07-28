using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Pyratron.PyraChat.IRC
{
    public class IRCClient
    {
        /// <summary>
        /// The port of the IRC server.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// The hostname of the IRC server.
        /// </summary>
        public string Host { get; }

        private readonly Thread networkThread;
        private readonly TcpClient tcpClient;
        private NetworkStream netStream;
        private StreamReader reader;
        private StreamWriter writer;

        /// <summary>
        /// Creates a new IRC client and connects to the specified host.
        /// </summary>
        /// <param name="host">Hostname.</param>
        /// <param name="port">Port.</param>
        public IRCClient(string host, int port)
        {
            Host = host;
            Port = port;

            tcpClient = new TcpClient();
            networkThread = new Thread(ProcessMessages);
        }

        public void Connect()
        {
            tcpClient.Connect(Host, Port);
            netStream = tcpClient.GetStream();

            reader = new StreamReader(netStream);
            writer = new StreamWriter(netStream)
            {
                NewLine = "\r\n",
                AutoFlush = true
            };

            networkThread.Start();

            writer.WriteLine("USER CyralTest 0 * :Cyral");
            writer.WriteLine("NICK CyralTest");
        }

        private void ProcessMessages()
        {
            while (tcpClient != null && tcpClient.Connected &&
                      reader != null && !reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;
                OnMessageReceived(line);
                if (line.StartsWith(":irc.frogbox.es NOTICE Auth :"))
                {
                    writer.WriteLine("JOIN #Pyratron");
                    writer.WriteLine("PRIVMSG #Pyratron :Test");
                }
            }
            OnMessageReceived("Loop exited.");
        }

        public delegate void MessageReceivedEventHandler(string message);
        public event MessageReceivedEventHandler MessageReceived;
        private void OnMessageReceived(string message) => MessageReceived?.Invoke(message);
    }
}