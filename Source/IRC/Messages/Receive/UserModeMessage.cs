using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// User mode message. (MODE)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.1"/>
    public class UserModeMessage : ReceivableMessage
    {
        public User User => BaseMessage.User;

        public UserModeMessage(Message msg) : base(msg)
        {
            if (msg.User.Nick == msg.Destination) //User and nick sent must match (RFC spec)
            {
                for (var i = 1; i < msg.Parameters.Length; i++)
                {
                    var mode = msg.Parameters[i];
                    if (string.IsNullOrWhiteSpace(mode)) continue;
                    if (mode[0].Equals('+'))
                        msg.User.AddMode(msg.Client, mode[1]);
                    else if (mode[0].Equals('+'))
                        msg.User.RemoveMode(msg.Client, mode[1]);
                }
                msg.Client.OnUserMode(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "MODE" && !msg.IsChannel;
        }
    }
}
