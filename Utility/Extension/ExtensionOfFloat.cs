
using System;
using System.Globalization;
namespace Lck.Utility.Extensions
{
    /// <summary>
    /// float double 的擴充方法
    /// </summary>
    public static class ExtensionOfFloat
    {



        /// <summary>
        /// 強制轉負數
        /// </summary>
        public static decimal ToForceMinusValue(this decimal number, int round = 2)
        {
            if (number > 0)
                number = number * -1;
                        
            //如果有指定要順便去小數點
            return Math.Round(number, round);
        }



        /// <summary>
        /// 取絕對值
        /// </summary>
        public static decimal ToAbsoluteValue(this decimal number , int? round = null)
        {
            if (number < 0)
                number = number * -1;

            if (round == null)
                return number;

            //如果有指定要順便去小數點
            return Math.Round(number, round.Value);
        }


        /// <summary>
        /// 取絕對值
        /// </summary>
        public static double ToAbsoluteValue(this double number, int? round = null)
        {
            if (number < 0)
                number = number * -1;

            if (round == null)
                return number;

            //如果有指定要順便去小數點
            return Math.Round(number, round.Value);
        }



        /// <summary>
        /// 顯示 +正數 -負數符號
        /// </summary>
        public static string ToPlusOrMinusMoneyFormat(this decimal number)
        {
            if (number > 0)
                return "+" + ToPriceString(number);
            else
                return ToPriceString(number);
        }


        

        /// <summary>
        /// 強制顯示負數符號
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToMinusMoneyFormat(this decimal number)
        {
            if (number > 0)
                return "-" + ToPriceString(number);
            else
                return ToPriceString(number);
        }



        
        /// <summary>
        /// 去除小數點
        /// </summary>
        public static decimal RemovePoint(this decimal number)
        {
            return Math.Floor(number);
        }
        

        /// <summary>
        /// 將數字轉成有千分位符號的string
        /// </summary>
        public static string ToPriceString(this decimal number)
        {
            if (number == 0)
                number = 0.00M;

            return number.ToString("N").Replace(".00", "");
        }



        /// <summary>
        /// 將數字轉成有千分位符號的string
        /// </summary>
        public static string ToPriceStringWithoutPoint(this decimal number)
        {
            number = RemovePoint(number);
            return number.ToString("N").Replace(".00", "");
        }


        public static string ToInvariantCulture(this double coordinateValue)
        {
            return coordinateValue.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 將金額轉為字串
        /// 小數點之後的數字，都是 0 就不顯示；不是 0 的話顯示到第二位，第三位四捨五入
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToMoneyFormatOfMail(this decimal money)
        {
            decimal roundMoney = Math.Round(money * 100, MidpointRounding.AwayFromZero) / 100;
            string strMoney = roundMoney.ToString("N2");
            var arrayMoney = strMoney.Split('.');
            if (arrayMoney[1] == "00")
            {
                return arrayMoney[0];
            }
            return strMoney;
        }
        public static bool IsNullOrDefault(this decimal? number)
        {
            if (number == null)
                return true;
            if (number == 0)
                return true;
            return false;
        }

        public static bool IsNullOrDefault(this double? number)
        {
            if (number == null)
                return true;
            if (number == 0)
                return true;
            return false;
        }

        /// <summary>
        /// 小數點 N 位後無條件捨去
        /// </summary>
        /// <param name="input"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static decimal DecimalFloor(decimal input, int digit = 2)
        {
            decimal baseNum = Convert.ToDecimal(Math.Pow(10, digit));
            input *= baseNum;
            //若 d 為負數 (產品端可能會傳回這樣的值), math.floor時會得到 -1
            if (input < 0) input = 0;
            return Math.Floor(input) / baseNum;
        }

        /// <summary>
        /// 檢查是否 input 的小數點限制, 小於等於 N 個 digit
        /// </summary>
        /// <param name="input"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static bool IsLessThanDigitN(decimal input, int digit)
        {
            //整數, 不需用字串比對, 30.000 用ExtensionOfString就會失敗
            if (input % 1 == 0)
                return true;
            else
                return ExtensionOfString.IsLessThanDigitN(input.ToString(), digit);
        }

    }


}