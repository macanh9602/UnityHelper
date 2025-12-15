using System;
using System.Globalization;
using UnityEngine;
namespace MyGame.Utils
{
    public static class DateTimeExtensions
    {
        // Định dạng chuẩn ISO 8601 để lưu file save/server
        private const string ISO_DATE_FORMAT = "yyyy-MM-dd";
        private const string FULL_DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Chuyển đổi chuỗi String thành DateTime an toàn.
        /// Luôn sử dụng InvariantCulture để tránh lỗi định dạng vùng miền.
        /// </summary>
        /// <param name="dateString">Chuỗi ngày tháng (dạng yyyy-MM-dd)</param>
        /// <returns>DateTime nếu thành công, hoặc DateTime.MinValue nếu lỗi</returns>
        public static DateTime ParseIsoDate(this string dateString)
        {
            if (DateTime.TryParseExact(dateString, ISO_DATE_FORMAT,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            Debug.LogError($"[TimeUtils] Không thể parse ngày: {dateString}. Trả về MinValue.");
            return DateTime.MinValue;
        }

        /// <summary>
        /// Chuyển DateTime thành String để lưu xuống JSON/Disk.
        /// </summary>
        public static string ToIsoString(this DateTime dateTime)
        {
            return dateTime.ToString(ISO_DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Tính số ngày chênh lệch giữa 2 mốc thời gian (bỏ qua giờ phút giây).
        /// Luôn trả về số dương.
        /// </summary>
        public static int DaysDifference(this DateTime fromDate, DateTime toDate)
        {
            return Mathf.Abs((toDate.Date - fromDate.Date).Days);
        }

        /// <summary>
        /// Chuyển đổi sang Unix Timestamp (giây) - Cần thiết khi làm việc với Server.
        /// </summary>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            // Mốc thời gian Unix (1/1/1970)
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(dateTime.ToUniversalTime() - epoch).TotalSeconds;
        }
    }
}

///example of usage
/*
string savedDate = "2023-12-01";
DateTime start = savedDate.ParseIsoDate(); // Extension method giúp gọi trực tiếp
int diff = start.DaysDifference(DateTime.UtcNow);
Debug.Log($"Số ngày đã trôi qua: {diff}");
*/
