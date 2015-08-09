using System;

namespace Pyratron.PyraChat.IRC
{
    /// <summary>
    /// Item containing data for a ban, invite, or exception.
    /// </summary>
    public class PermissionItem
    {
        public string Mask { get; }
        public DateTime Date { get; }

        /// <summary>
        /// The user that added this item.
        /// </summary>
        public string UserFrom { get; }

        public PermissionItem(string mask, DateTime date, string userFrom)
        {
            Mask = mask;
            Date = date;
            UserFrom = userFrom;
        }
    }
}