
using System;
using System.Collections.Generic;

namespace Lck.Utility.Extensions
{
    /// <summary>
    /// int的擴充方法
    /// </summary>
    public static class ExtensionOfInt
    {
        
        /// <summary>
        /// 布林資料
        /// </summary>
        public static int ToInteger(this bool booleanValue)
        {
            if (booleanValue)
                return 1;

            return 0;
        }



        /// <summary>
        /// 轉成縮寫過的數字
        /// </summary>
        public static string ToShortNumberString(this int number)
        {
            if (number > 1000000)
                return string.Format("{0}M", number / 1000000);

            if (number > 1000)
                return string.Format("{0}K", number / 1000);

            return number.ToString();
        }


        /// <summary>
        /// 將數字轉成有千分位符號的string
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToPriceString(this int number)
        {           
           return number.ToString("N").Replace(".00" , "");
        }

        /// <summary>
        /// 將數字轉成有千分位符號的string
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToPriceString(this long number)
        {          
            return number.ToString("N").Replace(".00", "");
        }


        /// <summary>
        /// 將數字換算成 時分文字
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToShortHourString(this int inputMinute)
        {
            if (inputMinute == 0)
                return string.Empty;

            int hour = inputMinute / 60;

            long minute = inputMinute % 60;

            if(hour <= 0)
                return string.Format(" {0} 分鐘", minute);

            //TODO 多語系
            return string.Format("{0} 小時 {1} 分鐘", hour, minute);
        }

        /// <summary>
        /// 將數字轉成長度5的字串，空白補零
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToPadLeftString(this int number ,int length = 6)
        {
            return number.ToString().PadLeft(length, '0');
        }

        /// <summary>
        /// 將數字轉成長度5的字串，空白補零
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToPadLeftString(this float number , int length =6)
        {
            return number.ToString().PadLeft(length, '0');
        }

        /// <summary>
        /// 將字串轉成長度5的字串，空白補零
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToPadLeftString(this string str)
        {
            return str.PadLeft(6, '0');
        }

        /// <summary>
        /// 將數字百分比轉換成可計算數字
        /// </summary>
        public static decimal ToPercentNumber(this int number)
        {
            return (decimal)number / 100;
        }

        /// <summary>
        /// 將數字百分比轉換成可計算數字
        /// </summary>
        public static decimal ToPercentNumber(this decimal number)
        {
            return number / 100;
        }


        ///to base-32
        public static string base10to32(this int number)
        {
            string hexnumbers = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
            string hex = "";
            int remainder;
            do
            {
                remainder = number % 32;
                number = number / 32;
                hex = hexnumbers[remainder] + hex;
            }
            while (number > 0);
            return hex;
        }

        public static bool IsNullOrDefault(this int? number)
        {
            if (number == null)
                return true;
            if (number == 0)
                return true;
            return false;
        }
    }


}