using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Lck.Utility.Extensions
{
    public static class ExtensionOfHttpRequestBase
    {        


        public static string GetQueryStringValueFromUrlFormat(this string url , string parameterName)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            if(url.Contains("?"))
            {
                url = url.Split('?')[1];
            }
        
            return HttpUtility.ParseQueryString(url).Get(parameterName);
        }

        
        public static bool IsFormUrlEncoded(this HttpRequest request)
        {
            return request.ContentType == "application/x-www-form-urlencoded";
        }

        public static string GetRequestBodyString(this HttpRequest request , bool passwordProtected = true)
        {
            try
            {
                if (request.HttpMethod == "GET")
                    return string.Empty;

                //有密碼的api 不紀錄requestBody
                if (passwordProtected && IsPasswordApiUrl(request.Url.AbsoluteUri))
                    return "** MASK **";

                if(IsFormUrlEncoded(request))
                {
                    string body = string.Empty;

                    if(request.Form.Keys == null ||
                       request.Form.Keys.Count == 0)
                    {
                        return string.Empty;
                    }

                    foreach(var key in request.Form.Keys)
                    {
                        string keyValue = key.ToString();
                        body += $"&{keyValue}={request.Form[keyValue]}";
                    }

                    if (body.FirstOrDefault() == '&')
                        body = body.Substring(1, body.Length - 1);

                    return body;
                }
                else
                {
                    return GetBodyStringFromInputstream(request.InputStream);
                }
            }
            catch
            {

            }

            return string.Empty;
        }

        
        public static string GetBodyStringFromInputstream(this Stream inputStream)
        {
            string requestBody = string.Empty;

            using (var stream = inputStream)
            using (var reader = new StreamReader(stream))
            {
                requestBody = reader.ReadToEnd();
            }

            return requestBody;
        }


        public static bool IsPasswordApiUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            url = url.ToLower();

            //有密碼相關的不紀錄
            if (url.Contains("password") ||
               url.Contains("authentication") ||
               url.Contains("register") ||
               url.Contains("users/self") ||
               url.Contains("login") ||
               url.Contains("signin") ||
               url.Contains("signup") ||
               url.Contains("pwd") ||
               url.Contains("psw") ||
               url.Contains("resetp"))
            {
                return true;
            }

            return false;
        }

    }

}
