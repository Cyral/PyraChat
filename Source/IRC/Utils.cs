using System;

namespace Pyratron.PyraChat.IRC
{
    /// <summary>
    /// Handy utility methods.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Converts a unix timestamp to a .NET DateTime.
        /// </summary>
        public static DateTime UnixTimeStampToDateTime(double unixTimestamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimestamp).ToLocalTime();
            return dateTime;
        }
    }
}