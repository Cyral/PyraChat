using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using Pyratron.PyraChat.IRC.Messages;
using Pyratron.PyraChat.IRC.Messages.Receive;
using Pyratron.PyraChat.IRC.Messages.Receive.Numerics;
using Pyratron.PyraChat.IRC.Messages.Send;
using JoinMessage = Pyratron.PyraChat.IRC.Messages.Receive.JoinMessage;
using PrivateMessage = Pyratron.PyraChat.IRC.Messages.Receive.PrivateMessage;

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
        public User User { get; }

        internal StringBuilder MOTDBuilder { get; set; } = new StringBuilder();

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
            Ping += message => Send(new PongMessage(message.Extra));
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
            Send(new Messages.Send.UserMessage(User));
            Send(new Messages.Send.NickMessage(User));
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        public void Send(SendableMessage message)
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
                    OnIRCMessage(msg);
            }
        }

        #region Events

        public delegate void IRCMessageEventHandler(Message message);

        public delegate void ChannelJoinEventHandler(JoinMessage message);

        public delegate void MessageEventHandler(PrivateMessage message);

        public delegate void PingEventHandler(PingMessage message);

        public delegate void NoticeEventHandler(NoticeMessage message);

        public delegate void ConnectEventHandler();

        public delegate void ReplyWelcomeEventHandler(WelcomeMessage message);

        public delegate void ReplyYourHostEventHandler(YourHostMessage message);

        public delegate void ReplyCreatedEventHandler(CreatedMessage message);

        public delegate void ReplyMyInfoEventHandler(MyInfoMessage message);

        public delegate void ReplyISupportEventHandler(SupportMessage message);

        public delegate void ReplyBounceEventHandler(BounceMessage message);

        public delegate void ReplyMOTDEndEventHandler(MOTDEndMessage message);

        public delegate void ReplyMOTDStartEventHandler(MOTDStartMessage message);

        public delegate void ReplyMOTDEventHandler(MOTDMessage message);

        public delegate void ReplyLUserClientEventHandler(LUserClientMessage message);

        /// <summary>
        /// General output logging message.
        /// </summary>
        public event IRCMessageEventHandler IRCMessage;

        public event ChannelJoinEventHandler ChannelJoin;
        public event MessageEventHandler Message;
        public event PingEventHandler Ping;
        public event NoticeEventHandler Notice;
        public event ConnectEventHandler Connect;
        public event ReplyWelcomeEventHandler ReplyWelcome;
        public event ReplyYourHostEventHandler ReplyYourHost;
        public event ReplyCreatedEventHandler ReplyCreated;
        public event ReplyMyInfoEventHandler ReplyMyInfo;
        public event ReplyISupportEventHandler ReplyISupport;
        public event ReplyBounceEventHandler ReplyBounce;
        public event ReplyMOTDEndEventHandler ReplyMOTDEnd;
        public event ReplyMOTDStartEventHandler ReplyMOTDStart;
        public event ReplyMOTDEventHandler ReplyMOTD;
        public event ReplyLUserClientEventHandler ReplyLUserClient;

        /// <summary>
        /// General output logging message.
        /// </summary>
        /// <param name="message">IRC message received.</param>
        internal void OnIRCMessage(Message message) => IRCMessage?.Invoke(message);

        internal void OnChannelJoin(JoinMessage message) => ChannelJoin?.Invoke(message);
        internal void OnMessage(PrivateMessage message) => Message?.Invoke(message);
        internal void OnPing(PingMessage message) => Ping?.Invoke(message);
        internal void OnNotice(NoticeMessage message) => Notice?.Invoke(message);
        internal void OnConnect() => Connect?.Invoke();
        internal void OnReplyWelcome(WelcomeMessage message) => ReplyWelcome?.Invoke(message);
        internal void OnReplyYourHost(YourHostMessage message) => ReplyYourHost?.Invoke(message);
        internal void OnReplyCreated(CreatedMessage message) => ReplyCreated?.Invoke(message);
        internal void OnReplyMyInfo(MyInfoMessage message) => ReplyMyInfo?.Invoke(message);
        internal void OnReplyISupport(SupportMessage message) => ReplyISupport?.Invoke(message);
        internal void OnReplyBounce(BounceMessage message) => ReplyBounce?.Invoke(message);
        internal void OnReplyMOTDEnd(MOTDEndMessage message) => ReplyMOTDEnd?.Invoke(message);
        internal void OnReplyMOTDStart(MOTDStartMessage message) => ReplyMOTDStart?.Invoke(message);
        internal void OnReplyMOTD(MOTDMessage message) => ReplyMOTD?.Invoke(message);
        internal void OnReplyLUserClient(LUserClientMessage message) => ReplyLUserClient?.Invoke(message);

        #endregion //Events
    }
}