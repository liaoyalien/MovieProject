
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Lck.Utility.Extensions
{
    public static class ExtensionOfException
    {

        /// <summary>
        ///取得 最底層的 inner exception
        /// </summary>
        public static Exception GetMostInnerException(this Exception ex)
        {
            int times = 0;

            Exception resultEx = ex;
            while (true)
            {
                times++;

                if (times >= 25)
                    break;

                if(resultEx.InnerException == null)
                {
                    break;
                }      
                else
                {
                    resultEx = resultEx.InnerException;
                }
            }

            return resultEx;
        }

        


        private static string TryGetReasonMessage(dynamic returnObj)
        {
            try
            {
                //如果有定義 Reason欄位
                return string.Format("({0})" ,returnObj.Reason);
            }
            catch
            {
            }

            try
            {
                //如果 returnObj本來就是 string
                return returnObj;
            }
            catch
            {

            }

            return string.Empty;
        }
        


        /// <summary>
        /// 取得程式碼錯在第幾行的重點資訊
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetErrorOccurLine(this Exception ex)
        {
            List<string> listResult = new List<string>();


            try
            {
                if (string.IsNullOrWhiteSpace(ex.StackTrace))
                    return string.Empty;

                var split = ex.StackTrace.Split(new string[] { "at " }, StringSplitOptions.None);

                foreach (var eachText in split)
                {
                    var splitAgain = eachText.Split(new string[] { @"\" }, StringSplitOptions.None);

                    foreach (var text in splitAgain)
                    {
                        if (text.Contains(".cs"))
                        {
                            listResult.Add(text.Truncate(150));
                        }
                    }
                }

            }
            catch
            {

            }

           
            return " " +  string.Join("", listResult);
        }
        

        /// <summary>
        /// 追溯兩層innerException 方便debug用
        /// </summary>
        public static string GetBebugInnerMessage(this Exception ex)
        {
            var lastException = GetMostInnerException(ex);

            string innserMsg = string.Empty;

            if (ex.InnerException != null)
                innserMsg = lastException.Message;

            string errorCodeLine = GetErrorOccurLine(lastException);
            
            return innserMsg + " , " + ex.Message.Truncate(150) + errorCodeLine;
        }



    }

}
