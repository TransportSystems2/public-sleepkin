using System;

namespace Pillow.ApplicationCore.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd hh:mm:ss");
        }

        public static DateTime? FromString(this string dateTimeString)
        {
            return DateTime.TryParse(dateTimeString, out var result) ? result : null;
        }
    }
}