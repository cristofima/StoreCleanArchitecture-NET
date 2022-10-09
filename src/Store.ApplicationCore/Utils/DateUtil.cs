using System;

namespace Store.ApplicationCore.Utils
{
    public static class DateUtil
    {
        public static DateTime GetCurrentDate()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
        }
    }
}
