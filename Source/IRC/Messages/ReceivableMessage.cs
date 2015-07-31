using System.IO;

namespace Pyratron.PyraChat.IRC.Messages
{
    /// <summary>
    /// A receivable message.
    /// </summary>
    public abstract class ReceivableMessage
    {
        /// <summary>
        /// The original message received.
        /// </summary>
        public Message BaseMessage { get; }

        public ReceivableMessage(Message msg)
        {
            BaseMessage = msg;
        }

        /*
         To add a new message, add a similar line to Message.cs:
         if (PingMessage.CanProcess(this)) return new PingMessage(this);
         
         Make sure to add static CanProcess method to the message:
         public static bool CanProcess(Message msg)
         {
              return msg.Type == "PING";
         }
         */
    }
}