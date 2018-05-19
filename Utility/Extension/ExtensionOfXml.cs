using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Xml;

namespace Lck.Utility.Extensions
{
    public static class ExtensionOfXml
    {

        /// <summary>
        /// 创建XML
        /// </summary>
        public static XmlDocument ConvertToXML(this string strXML)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.LoadXml(strXML);

                return xmlDoc;
            }
            catch (Exception ex)
            {

            }

            return null;
        }


    }

}
