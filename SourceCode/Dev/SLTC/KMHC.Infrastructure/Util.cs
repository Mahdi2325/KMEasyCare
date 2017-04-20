using EncryptKey;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace KMHC.Infrastructure
{
    public static class Util
    {

        public static string Encryption(string encryptStr)
        {
            Encryption en = new Encryption(EncryptionAlgorithm.TripleDes);
            return en.encrypt(encryptStr);
        }
   
        public static T Clone<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }

        public static int GetAgeByDate(DateTime? birthDay)
        {
            int age = 0;
            if (birthDay.HasValue)
            {
                age = DateTime.Now.Year - birthDay.Value.Year;
                if (DateTime.Now.Month < birthDay.Value.Month || (DateTime.Now.Month == birthDay.Value.Month && DateTime.Now.Day < birthDay.Value.Day)) age--;
                if (age <= 0)
                {
                    return 0;
                }
                return age;
            }
            return age;
        }


        /// <summary>
        /// List扩展方法，将List元素用分隔符连接后，返回字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static String Join<T>(this List<T> list, string splitStr = ",")
        {
            string result = string.Empty;
            foreach (var item in list)
            {
                result += item.ToString() + splitStr;
            }
            return result.Trim(splitStr.ToCharArray());
        }

        public static Boolean validPdfParams(String pdfFilePath, String doc, String page)
        {
            return doc != null
                && !(doc.Length > 255 || page != null && page.Length > 255)
                && doc.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) == -1
                && ((page != null && page.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) == -1) || page == null)
                && doc.IndexOf('\\') == -1 && doc.IndexOf('/') == -1
                && ((page != null && page.IndexOf('\\') == -1 && page.IndexOf('/') == -1) || page == null)
                && ((page != null && (doc + page).IndexOf("..") == -1) || page == null)
                && ((page != null && (doc + page).IndexOf("cmd.") == -1 && (doc + page).IndexOf(".exe") == -1) || page == null);
        }

        public static Boolean validSwfParams(String swfFilePath, String doc, String page)
        {
            return doc != null && !(doc.Length > 255 || page != null && page.Length > 255) && doc.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) == -1 && page.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) == -1 && doc.IndexOf('\\') == -1 && doc.IndexOf('/') == -1 && page.IndexOf('\\') == -1 && page.IndexOf('/') == -1 && (doc + page).IndexOf("..") == -1 && (doc + page).IndexOf("cmd.") == -1 && (doc + page).IndexOf(".exe") == -1;
        }

        public static int getTotalPages(string fileName)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(fileName)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }

        public static void setCacheHeaders(HttpContext context)
        {
            context.Response.AddHeader("Cache-Control", "private, max-age=10800, pre-check=10800");
            context.Response.AddHeader("Pragma", "private");
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(2));
        }

        public static string ToTwDate(DateTime? date)
        {
            if (date.HasValue)
            {
                return string.Format("{0}/{1}/{2}", date.Value.Year, date.Value.Month, date.Value.Day);
                //return string.Format("{0}/{1}/{2}", date.Value.Year, date.Value.Month, date.Value.Day);
            }
            else
            {
                return "";
            }
        }


        public static string ToTwDate(DateTime? date, string type)
        {
            if (date.HasValue)
            {
                return Convert.ToDateTime(date).ToString("yyyy/MM/dd HH:mm");
            }
            else
            {
                return "";
            }
        }

        public static string ToTimelineDate(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                DateTime dt;
                if (DateTime.TryParse(date, out dt))
                {
                    return string.Format("{0},{1},{2}", dt.Year, dt.Month, dt.Day);
                }
            }
            return "";
        }

        public static void DownloadFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileInfo.Name);
            HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            HttpContext.Current.Response.WriteFile(fileInfo.FullName);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        //public static string connectStr = Util.FetchConnectStr("TWSLTC", "192.168.10.149", "sltc", "sltc123$");
        // public static string connectStr = Util.FetchConnectStr("TWSLTC", "kangmei-sz.imwork.net", "sltc", "sltc");
        public static string connectStr = Util.FetchConnectStr("JLSLTC", "10.133.210.243", "sltc", "sltc4ever%love");
        public static string FetchConnectStr(string dbName, string ip, string userId, string password)
        {
            string connectStr = "metadata=res://*/MysqlDataContext.csdl|res://*/MysqlDataContext.ssdl|res://*/MysqlDataContext.msl;provider=MySql.Data.MySqlClient;provider connection string=\";server="
                                + ip + ";database="
                                + dbName + ";persist security info=True;user id="
                                + userId + ";password="
                                + password + "\";";


            return connectStr;
        }


        /// <summary>
        /// 获取web客户端ip
        /// </summary>
        /// <returns></returns>
        public static string GetWebClientIp()
        {

            string userIP = "未获取用户IP";

            try
            {
                if (System.Web.HttpContext.Current == null
                 || System.Web.HttpContext.Current.Request == null
                 || System.Web.HttpContext.Current.Request.ServerVariables == null)
                {
                    return "";
                }

                string CustomerIP = "";

                //CDN加速后取到的IP simone 090805
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!String.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (CustomerIP == null)
                    {
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.Compare(CustomerIP, "unknown", true) == 0 || String.IsNullOrEmpty(CustomerIP))
                {
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                return CustomerIP;
            }
            catch { }

            return userIP;

        }
        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="dateBegin">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public static TimeSpan ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            return ts1.Subtract(ts2).Duration();
        }
        /// <summary>
        /// 获取枚举值上的Description特性的说明
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">枚举值</param>
        /// <returns>特性的说明</returns>
        public static string GetEnumDescription<T>(T obj)
        {
            var type = obj.GetType();
            FieldInfo field = type.GetField(Enum.GetName(type, obj));
            DescriptionAttribute descAttr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (descAttr == null)
            {
                return string.Empty;
            }

            return descAttr.Description;
        }

        ///// <summary>
        ///// 32位MD5加密
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static string Md5(string input)
        //{
        //    var md5Hash = new MD5CryptoServiceProvider();
        //    var data = md5Hash.ComputeHash(Encoding.Default.GetBytes(input));
        //    var sBuilder = new StringBuilder();
        //    foreach (var t in data)
        //    {
        //        sBuilder.Append(t.ToString("x2"));
        //    }
        //    return sBuilder.ToString();
        //}
        #region 对象的深拷贝
        public static T DeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
        #endregion
    }
}
