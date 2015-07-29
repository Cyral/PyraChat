using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SqlServer.Server;
using Pyratron.PyraChat.IRC.Messages;
using Pyratron.PyraChat.IRC.Messages.Send;

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
        /// Creates a new IRC client.
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

            //Register neccessary internal events
            Ping += (client, message) => SendMessage(new PongMessage(message));
        }

        /// <summary>
        /// Connects to the server.
        /// </summary>
        public void Start()
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

            //Send user information
            SendMessage(new UserMessage(User));
            SendMessage(new NickMessage(User));
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
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

                //Parse the message
                var msg = new Message(this, line);
                //Choose a message type and process the message
                msg.Process();
                OnMessageReceived(line);
            }
            OnMessageReceived("Loop exited.");
        }

        public delegate void MessageReceivedEventHandler(string message);
        public event MessageReceivedEventHandler MessageReceived;
        private void OnMessageReceived(string message) => MessageReceived?.Invoke(message);

        #region Events

        public delegate void ChannelJoinEventHandler(Client client, Channel channel);
        public delegate void MessageEventHandler(Client client, User user, string message);
        public delegate void PingEventHandler(Client client, string message);
        public delegate void NoticeEventHandler(Client client, User user, string noticeMessage);
        public delegate void ConnectEventHandler(Client client);
        public delegate void WelcomeEventHandler(Client client, string welcomeMessage);

        public event ChannelJoinEventHandler ChannelJoin;
        public event MessageEventHandler Message;
        public event PingEventHandler Ping;
        public event NoticeEventHandler Notice;
        public event ConnectEventHandler Connect;
        public event WelcomeEventHandler Welcome;

        internal void OnChannelJoin(Channel channel) => ChannelJoin?.Invoke(this, channel);
        internal void OnMessage(User user, string message) => Message?.Invoke(this, user, message);
        internal void OnPing(string message) => Ping?.Invoke(this, message);
        internal void OnNotice(User user, string noticeMessage) => Notice?.Invoke(this, user, noticeMessage);
        internal void OnConnect() => Connect?.Invoke(this);
        internal void OnWelcome(string welcomeMessage) => Welcome?.Invoke(this, welcomeMessage);

        #endregion //Events
    }
}