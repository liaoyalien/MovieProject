using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;


namespace Lck.Utility.Extensions
{
    /// <summary>
    /// Datetime 的擴充方法
    /// </summary>
    public static class ExtensionOfDatetime
    {

        public static  List<DateTime> GetEachDateOnDuration(this DateTime begin , DateTime end , int intervalMinute = 1440)
        {
            List<DateTime> allDays = new List<DateTime>();

            int offset = -1;
            while(true)
            {
                offset++;

                DateTime beginOfThis = begin.AddMinutes(offset * intervalMinute);

                if (beginOfThis > end)
                    break;

                allDays.Add(beginOfThis);
            }


            return allDays;
        }


        public static DateTime To_23_59_59(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 23, 59, 59 , 999);
        }


        public static string ToDashShortDateString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }
        

        /// <summary>
        /// 注意裡面的時間是 +0時區的
        /// </summary>
        public static long CovertToUnixTimestamp(this DateTime? utcTime)
        {
            return (long)utcTime.Value.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        
        public static double CovertToUnixTimestamp(this DateTime utcTime)
        {
            return (long)utcTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        

        /// <summary>
        /// 注意裡面的時間是 +0時區的
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(this double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        

    }
    

}