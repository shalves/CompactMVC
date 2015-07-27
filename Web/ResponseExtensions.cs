using System.Json;
using System.Text;
using System.Web.UI;

namespace System.Web
{
    public static class ResponseExtensions
    {
        /// <summary>
        /// 发送301响应（永久性重定向）到客户端
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="url">目标Url</param>
        public static void Send301(this HttpResponseBase Response, string url)
        {
            Response.Clear();
            Response.StatusCode = 301;
            Response.StatusDescription = "Moved Permanently";
            Response.AppendHeader("Location", url);
            Response.Write(string.Format("Object Moved Permanently, Location: {0}", url));
            Response.End();
        }

        /// <summary>
        /// 发送401响应（请求未授权）到客户端
        /// </summary>
        /// <param name="Response"></param>
        public static void Send401(this HttpResponseBase Response)
        {
            Response.Clear();
            Response.StatusCode = 401;
            Response.StatusDescription = "Unauthorized";
            Response.Write("Request Unauthorized");
            Response.End();
        }

        /// <summary>
        /// 发送403响应（禁止访问）到客户端
        /// </summary>
        /// <param name="Response"></param>
        public static void Send403(this HttpResponseBase Response)
        {
            Response.Clear();
            Response.StatusCode = 403;
            Response.StatusDescription = "Forbidden";
            Response.Write("Request Forbiddened");
            Response.End();
        }

        /// <summary>
        /// 发送404响应（资源不存在）到客户端
        /// </summary>
        /// <param name="Response"></param>
        public static void Send404(this HttpResponseBase Response)
        {
            Response.Clear();
            Response.StatusCode = 404;
            Response.StatusDescription = "Not Found";
            Response.Write("Object Not Found");
            Response.End();
        }

        /// <summary>
        /// 发送405响应（不接受请求类型）到客户端
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="allow"></param>
        public static void Send405(this HttpResponseBase Response, HttpVerb allow)
        {
            Response.Clear();
            Response.StatusCode = 405;
            Response.StatusDescription = "Method Not Allowed";
            Response.AppendHeader("Allow", allow.ToString());
            Response.Write(string.Format("Method Not Allowed, Except: {0}", allow.ToString()));
            Response.End();
        }

        /// <summary>
        /// 输出纯文本到客户端
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="text"></param>
        public static void WriteText(this HttpResponseBase Response, string text)
        {
            Response.ContentType = "text/plain";
            Response.ClearContent();
            Response.BinaryWrite(Encoding.UTF8.GetBytes(text));
            Response.Flush();
        }

        /// <summary>
        /// 输出JS文本到客户端
        public static void WriteJavaScript(this HttpResponseBase Response, JavaScript script)
        {
            Response.ContentType = "text/html";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(script.ToString());
        }

        /// <summary>
        /// 输出Json文本到客户端
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="json"></param>
        public static void WriteJson(this HttpResponseBase Response, IJson json)
        {
            Response.ContentType = "application/json";
            Response.ClearContent();
            Response.BinaryWrite(Encoding.UTF8.GetBytes(json.ToString()));
            Response.Flush();
        }

        /// <summary>
        /// 输出Jsonp文本到客户端
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="callBack">客户端的js回调方法</param>
        /// <param name="json">json参数</param>
        public static void WriteJsonp(this HttpResponseBase Response, string callBack, IJson json)
        {
            Response.ContentType = "application/x-javascript";
            Response.ClearContent();
            Response.BinaryWrite(Encoding.UTF8.GetBytes(string.Format("{0}({1});", callBack, json.ToString())));
            Response.Flush();
        }
    }
}
