using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;


namespace Lck.Kernel
{
    /// <summary>
    /// HTTP操作类
    /// </summary>
    public class HTTP
    {
        public int statusCode;
        public int timeOut = 60000;                 //60秒
        public string HTML = string.Empty;
        public string errorString = string.Empty;
        
        public string contentType = string.Empty;   //文件类型
        public long contentLength = 0;              //文件大小
        public string responseUrl = string.Empty;   //请求的url

        public string character = string.Empty;

        public bool isXMLHttpRequest = false;

        public string userAgent;
        public string accept;

        HttpWebRequest webRequest;
        HttpWebResponse webResponse;

        public CookieCollection cc = new CookieCollection();

        public bool isSucc = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public HTTP()
        {
            this.userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; InfoPath.2)";
            this.accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";

        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~HTTP()
        {

        }


        /// <summary>
        /// Post数据
        /// </summary>
        public string PostData(string URL, string Data)
        {
            HTML = string.Empty;
            contentType = string.Empty;
            statusCode = 0;

            System.Net.ServicePointManager.Expect100Continue = false;
            try
            {
                webRequest = HttpWebRequest.Create(URL) as HttpWebRequest;
                webRequest.Accept = this.accept;
                webRequest.UserAgent = this.userAgent;
                webRequest.Method = "POST";
                webRequest.Timeout = this.timeOut;

                if (character == string.Empty)
                {
                    character = "UTF-8";
                }

                CookieContainer mc = new CookieContainer();
                for (int i = 0; i < cc.Count; i++)
                {
                    cc[i].Domain = webRequest.RequestUri.Host;
                }
                mc.Add(cc);
                webRequest.CookieContainer = mc;
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                webRequest.Headers.Add("Accept-Language", "zh-cn");
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.Headers.Add("UA-CPU", "x86");

                if (isXMLHttpRequest)
                {
                    webRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                }

                webRequest.KeepAlive = false;
                webRequest.AllowAutoRedirect = true;
                webRequest.MaximumAutomaticRedirections = 5;

                Encoding encoding = Encoding.GetEncoding(character);
                byte[] byteData = encoding.GetBytes(Data);
                webRequest.ContentLength = byteData.Length;

                //发送数据
                Stream stream = webRequest.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length); ;
                stream.Close();

                //收接数据
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                statusCode = (int)webResponse.StatusCode;
                contentLength = webResponse.ContentLength;
                contentType = webResponse.ContentType;

                cc.Add(webResponse.Cookies);
                cc.Add(mc.GetCookies(webRequest.RequestUri));

                Stream receiveStream = null;
                string reContentEncoding = webResponse.ContentEncoding;//取得返回的编码方式
                if (reContentEncoding == "gzip")
                {
                    receiveStream = new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
                }
                else if (reContentEncoding == "deflate")
                {
                    receiveStream = new DeflateStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    receiveStream = webResponse.GetResponseStream();
                }

                StreamReader streamReader = new StreamReader(receiveStream, encoding);
                HTML = streamReader.ReadToEnd();


                //转化为UTF-8
                Encoding utf8 = Encoding.Unicode;
                byte[] responseEncodingBytes = encoding.GetBytes(HTML);
                byte[] utf8Bytes = Encoding.Convert(encoding, utf8, responseEncodingBytes);

                HTML = utf8.GetString(utf8Bytes);

                receiveStream.Close();
                streamReader.Close();
                webResponse.Close();

                isSucc = true;

            }
            catch (Exception ex)
            {
                isSucc = false;
                errorString = ex.Message;
            }
            finally
            {
            }

            return HTML;
        }

        /// <summary>
        /// Get数据
        /// </summary>
        public string GetData(string URL)
        {
            HTML = string.Empty;        
            contentType = string.Empty;
            statusCode = 0;

            //防止出错
            System.Net.ServicePointManager.Expect100Continue = false;

            try
            {
                webRequest = HttpWebRequest.Create(URL) as HttpWebRequest;
                webRequest.Accept = this.accept;
                webRequest.UserAgent = this.userAgent;
                webRequest.Method = "GET";
                webRequest.Timeout = this.timeOut;

                CookieContainer mc = new CookieContainer();
                for (int i = 0; i < cc.Count; i++)
                {
                    cc[i].Domain = webRequest.RequestUri.Host;
                }
                mc.Add(cc);
                webRequest.CookieContainer = mc;

                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Headers.Add("Accept-Encoding", "gzip,deflate");
                webRequest.Headers.Add("Accept-Language", "zh-cn");
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.Headers.Add("UA-CPU", "x86");

                if (isXMLHttpRequest)
                {
                    webRequest.Headers.Add("x-requested-with", "XMLHttpRequest");
                }

                webRequest.KeepAlive = false;


                webResponse = (HttpWebResponse)webRequest.GetResponse();


                cc.Add(webResponse.Cookies);
                cc.Add(mc.GetCookies(webRequest.RequestUri));


                if (webResponse.ContentLength == 0)
                    return HTML;

                Encoding encoding = Encoding.UTF8;
                if (this.character != string.Empty)
                {
                    encoding = Encoding.GetEncoding(character);
                }
                else
                {
                    if (webResponse.CharacterSet.Trim() != "")
                        encoding = Encoding.GetEncoding(webResponse.CharacterSet);
                }

                contentType = webResponse.Headers["Content-Type"];

                Stream receiveStream = null;

                //取得返回的编码方式
                string reContentEncoding = webResponse.ContentEncoding;
                if (reContentEncoding == "gzip")
                {
                    receiveStream = new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
                }
                else if (reContentEncoding == "deflate")
                {
                    receiveStream = new DeflateStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    receiveStream = webResponse.GetResponseStream();
                }

                StreamReader streamReader = new StreamReader(receiveStream, encoding);

                if (contentType.StartsWith("text", StringComparison.CurrentCultureIgnoreCase))
                    HTML = streamReader.ReadToEnd();


                //转化为UTF-8
                Encoding utf8 = Encoding.Unicode;
                byte[] responseEncodingBytes = encoding.GetBytes(HTML);
                byte[] utf8Bytes = Encoding.Convert(encoding, utf8, responseEncodingBytes);

                HTML = utf8.GetString(utf8Bytes);

                streamReader.Close();
                webResponse.Close();

                isSucc = true;

            }
            catch (Exception ex)
            {
                isSucc = false;
                errorString = ex.Message;
            }
            finally
            {
            }

            return HTML;
        }


        public T SendData<T>(string method, string url, string data, string token= "", Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = method;
            request.Timeout = 30000;
           
            if (method.ToUpper().Trim() == "POST")
            {
                request.ContentType = "application/json";                
                byte[] byteData = encoding.GetBytes(data);
                Stream stream = request.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length);
                stream.Close();
            }
           
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = null;
            receiveStream = response.GetResponseStream();
            
            StreamReader streamReader = new StreamReader(receiveStream, encoding);

            string revData = string.Empty;
            revData = streamReader.ReadToEnd();
            
            receiveStream.Close();
            streamReader.Close();
            response.Close();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(revData);
        }
    }
}
