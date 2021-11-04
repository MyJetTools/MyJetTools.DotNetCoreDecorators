using System;

namespace DotNetCoreDecorators
{

    public static class DateTimeUtils
    {
        private static readonly DateTime UnixBaseDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
        public static long UnixTime(this DateTime value)
        {
            return (long)(value - UnixBaseDateTime).TotalMilliseconds;
        }
        
        public static DateTime UnixTimeToDateTime(this long unixTime)
        {
            return UnixBaseDateTime.AddMilliseconds(unixTime);
        }
        
    }

}