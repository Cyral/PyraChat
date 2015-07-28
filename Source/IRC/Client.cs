using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Pyratron.PyraChat.IRC.Messages;

namespace Pyratron.PyraChat.IRC
{
    public class Client
    {
        /// <summary>
        /// The port of the IRC server.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// The hostname of the IRC server.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// The user connected to this network.
        /// </summary>
        public User User { get; private set; }

        public List<Channel> Channels { get; }

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
        /// <param name="user">User to connect with.</param>
        public Client(string host, int port, User user)
        {
            Channels = new List<Channel>();
            Host = host;
            Port = port;
            User = user;

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

            SendMessage(new UserMessage(User));
            SendMessage(new NickMessage(User));
        }

        public void SendMessage(ISendable message)
        {
            message.Send(writer, this);
        }

        private void ProcessMessages()
        {
            while (tcpClient != null && tcpClient.Connected && reader != null && !reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;

                var msg = new Message(this, line);
                OnMessageReceived($"{msg.Prefix} {msg.Type} {msg.Destination} {string.Join(" ", msg.Parameters)}");
            }
            OnMessageReceived("Loop exited.");
        }

        public delegate void MessageReceivedEventHandler(string message);
        public event MessageReceivedEventHandler MessageReceived;
        private void OnMessageReceived(string message) => MessageReceived?.Invoke(message);

        #region Events

        public delegate void NoticeEventHandler(Client client, User user, string notice);
        public event NoticeEventHandler Notice;
        internal void OnNotice(User user, string notice) => Notice?.Invoke(this, user, notice);

        #endregion //Events
    }
}