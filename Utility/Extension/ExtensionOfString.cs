using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace Lck.Utility.Extensions
{
    /// <summary>
    /// string的擴充方法
    /// </summary>
    public static class ExtensionOfString
    {
        public static bool IsJson(this string jsonText)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
            {
                return false;
            }
            var value = jsonText.Trim();

            if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                (value.StartsWith("[") && value.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(value);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }

            return false;
        }

        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        public static Nullable<T> TryToEnum<T>(this string str) where T : struct
        {
            T result = default(T);
            if(Enum.TryParse(str,true, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// 下面那個 Base64UrlEncode 建議不要再用了 轉出來的結果微妙的不同
        /// </summary>
        public static string ToBase64String(this string str)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("utf-8").GetBytes(str);
            string result = Convert.ToBase64String(bytes);
            return result;
        }

        public static string Base64UrlEncode(this string str)
        {
            byte[] encbuff = Encoding.UTF8.GetBytes(str);
            return HttpServerUtility.UrlTokenEncode(encbuff);
        }

        public static string Base64UrlDecode(this string str)
        {
            byte[] decbuff = HttpServerUtility.UrlTokenDecode(str);
            return Encoding.UTF8.GetString(decbuff);
        }
        

        /// <summary>
        /// 取得兩個文字中間的所有文字
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefixWord"></param>
        /// <param name="suffixWord"></param>
        /// <returns></returns>
        public static string GetInnerTextBetweenTwoWord(this string input, string prefixWord , string suffixWord)
        {
            int pFrom = input.IndexOf(prefixWord) + prefixWord.Length;
            int pTo = input.LastIndexOf(suffixWord);
            string result = input.Substring(pFrom, pTo - pFrom);
            return result;
        }

        public static string ToCamelCase<T>(this T input,int count = 1) where T : struct
        {
            return ToCamelCase(input.ToString(), count);
        }

        public static string ToCamelCase(this string input, int count = 1)
        {
            // 若字數不足
            if (input == null || input.Length < count)
                return input;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (i < count)
                {
                    sb.Append(Char.ToLower(input[i]));
                }
            }
            sb.Append(input.Substring(count, input.Length - count));
            return sb.ToString();
        }

        /// <summary>
        /// 從最後一個字開始取 取N個字 (預設值為一個字)
        /// </summary>
        public static string GetLastText(this string input, int previousHowMany = 1)
        {
           return input.Substring(input.Length - previousHowMany);
        }


        public static string ReplaceNonNumeric(this string input)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(input, "");
        }
        

        public static string UnicodeToString(this string srcText)
        {
            string dst = "";
            string src = srcText;
            int len = srcText.Length / 6;

            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }


        /// <summary>
        /// 幫重要隱私資訊 後半段打上馬賽克
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMaskPrivateText(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            if (input.Length <= 2)
                return input;

            return string.Format("{0}*******" ,  input.Truncate(2, ""));
        }

        /// <summary>
        /// 幫重要隱私資訊
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex">起始字元位置</param>
        /// <param name="length">長度</param>
        /// <param name="c">隱藏顯示字(預設為星號)</param>
        /// <returns></returns>
        public static string GetMaskPrivateText(this string input,int startIndex,int length,char c = '*')
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            
            if (input.Length < startIndex + length)
                return input;

            StringBuilder sb = new StringBuilder();
            sb.Append(input.Substring(0, startIndex));
            sb.Append(c, length);
            sb.Append(input.Substring(startIndex + length, input.Length - startIndex - length));

            return sb.ToString();
        }
        

        public static string ReplaceChangeLineToBRTag(this string value)
        {
            return value.Replace("\n", "<br />");
        }


        public static bool IsEmailFormat(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (value.Contains("@") == false)
                return false;

            if (value.Count(x => x == '@') >= 2)
                return false;

            var split = value.Split('@')
                .Where(x => string.IsNullOrWhiteSpace(x) == false)
                .ToList();


            if (split.Count() <= 1)
                return false;

            if (split.Count() > 2)
                return false;

            //字數太少
            if (split[0].Length <= 1)
                return false;

            if (split[1].Length <= 2)
                return false;
            

            return true;
        }

        public static int TryToInt(this string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }

        public static T TryConvertType<T>(this string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return default(T);

                if (typeof(T) == typeof(TimeSpan))
                    return TryParseTimeSpan(value);

                return (T)Convert.ChangeType(value, typeof(T));

            }
            catch
            {
                return default(T);
            }
        }


        private static dynamic TryParseTimeSpan(string value)
        {
            try
            {
                return TimeSpan.Parse(value);
            }
            catch
            {
                return new TimeSpan();
            }
        }
        


        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="source"></param>
        /// <returns>加密後的密碼</returns>
        public static string MD5Hash(this string source)
        {
            return string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(source)).Select(s => s.ToString("x2")));
        }



        /// <summary>
        /// 取代多餘的斜線符號
        /// </summary>
        public static string ReplaceDuplicateUrlSymbol(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;


            //求高手幫忙改成 regex
            //其實就是想把 http://www.google.com////myTrip  多餘的斜線符號取代掉
            if (source.Contains("://"))
            {
                source = source.Replace("://", "TEMP_STRING");
            }

            source = source.Replace("///", "/").Replace("//", "/");
            source = source.Replace("TEMP_STRING", "://");
            return source;
        }
        

        /// <summary>
        /// 取代掉所有特殊符號
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ReplaceAllSymbol(this string source, string replaceToWhat = "")
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            return Regex.Replace(source, @"[\W_]+", replaceToWhat);
        }


        /// <summary>
        /// 取代 除了空白符號以外的特殊符號
        /// </summary>
        public static string ReplaceAllSymbolExceptWhiteSpace(this string source, string replaceToWhat = "")
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;


            //空白符號、句號、井號 不取代
            string defaultReplaceTempText = "ReplaceTempString";
            string replaceTextOfPeriodSymbol = "replaceTextOfPeriodSymbol";
            string replaceTextOfHashSymbol = "replaceTextOfHashSymbol";

            source = source.Replace(" ", defaultReplaceTempText);
            source = source.Replace(".", replaceTextOfPeriodSymbol);
            source = source.Replace("#", replaceTextOfHashSymbol);


            source = source.ReplaceAllSymbol(replaceToWhat);
            source = source.Replace(defaultReplaceTempText, " ");
            source = source.Replace(replaceTextOfPeriodSymbol, ".");
            source = source.Replace(replaceTextOfHashSymbol, "#");

            return source;
        }

        


        /// <summary>
        /// 判斷輸入是否為日文五十音
        /// </summary>
        /// <param name="strChinese"></param>
        /// <returns></returns>
        public static bool IsJapanese50WordOnly(this string inputText)
        {
            if (IsJapanese(inputText) && IsChinese(inputText) == false)
                return true;

            return false;
        }


        /// <summary>
        /// 判斷輸入是否為韓文
        /// </summary>
        /// <param name="strChinese"></param>
        /// <returns></returns>
        public static bool IsKoreanWord(this string inputText)
        {
            bool sss1 = AnyCharsInRange(inputText, 0x1100, 0x11F);
            bool sss2 = AnyCharsInRange(inputText, 0x3130, 0x318F);
            bool sss3 = AnyCharsInRange(inputText, 0xAC00, 0xD7AF);
            return sss1 || sss2 || sss3;
        }



        /// <summary>
        /// 判斷輸入是否為日文
        /// </summary>
        /// <param name="strChinese"></param>
        /// <returns></returns>
        public static bool IsJapanese(this string inputText)
        {

            bool sss1 = AnyCharsInRange(inputText, 0x3040, 0x309F);
            bool sss2 = AnyCharsInRange(inputText, 0x30A0, 0x30FF);
            bool sss3 = AnyCharsInRange(inputText, 0x4E00, 0x9FBF);
            return sss1 || sss2 || sss3;

        }

        

        private static bool AnyCharsInRange(string text, int min, int max)
        {
            return text.Any(e => e >= min && e <= max);
        }



        /// <summary>
        /// 判斷輸入是否為中文
        /// ps: 中英混合 也算true
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public static bool IsChinese(this string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return false;

            var bresult = true;
            var dRange = 0;
            var dstringmax = Convert.ToInt32("9fff", 16);
            var dstringmin = Convert.ToInt32("4e00", 16);

            for (var i = 0; i < inputText.Length; i++)
            {
                dRange = Convert.ToInt32(Convert.ToChar(inputText.Substring(i, 1)));
                if (dRange >= dstringmin && dRange < dstringmax)
                {
                    bresult = true;
                    break;
                }
                else
                {
                    bresult = false;
                }
            }

            return bresult;
        }
        

        /// <summary>
        /// 取出前N個字，並在最後加上 ...
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <param name="truncation"></param>
        /// <returns></returns>
        public static string Truncate(this string s, int length = 30, string truncation = "")
        {
            if (string.IsNullOrWhiteSpace(s))
                return string.Empty;

            return s.Length > length ?
                s.Substring(0, length - truncation.Length) + truncation : s;
        }


        /// <summary>
        /// 將 HTML tag 移除
        /// </summary>
        public static string StripHTML(this string htmlString)
        {
            if (string.IsNullOrEmpty(htmlString))
                htmlString = string.Empty;

            const string patternTag = @"<(.|\n)*?>";
            const string patternAscii = @"&((.[a-zA-Z])*);";

            return Regex.Replace(Regex.Replace(htmlString, patternTag, string.Empty), patternAscii, string.Empty).HtmlDecode();
        }



        /// <summary>
        /// 將 html 4.0 Special Entities移除
        /// </summary>
        public static string HtmlDecode(this string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return string.Empty;

            return HttpUtility.HtmlDecode(inputString);
        }


        /// <summary>
        /// 反轉字串
        /// </summary>
        public static string Reverse(this string s)
        {
            if (String.IsNullOrEmpty(s))
                return string.Empty;

            var charArray = new char[s.Length];
            var len = s.Length - 1;
            for (var i = 0; i <= len; i++)
            {
                charArray[i] = s[len - i];
            }
            return new string(charArray);
        }


        /// <summary>
        /// 判斷string是否為數字
        /// </summary>
        public static bool IsNumeric(this string s)
        {
            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(s), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }


        /// <summary>
        /// 判斷string是否為Guid
        /// </summary>
        public static bool IsGuid(this string s)
        {

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is null.
            Guid retGuid;

            bool isGuid = Guid.TryParse(Convert.ToString(s), out retGuid);
            return isGuid;
        }
        

        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source) ;
        }

        /// <summary>
        /// 檢查 0 - N 個 digit
        /// </summary>
        /// <param name="input"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static bool IsLessThanDigitN(string input, int digit)
        {
            string pattern = "^(-{0,1})[0-9]+(.[0-9]{0," + digit.ToString() + "})?$";
            return Regex.IsMatch(input, pattern) ? true : false;
        }


    }


}