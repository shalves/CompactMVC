using System;

namespace System.Web
{
    /// <summary>
    /// 表示HTTP请求所使用的方法谓词
    /// </summary>
    [Serializable]
    [Flags]
    public enum HttpVerb
    {
        /// <summary>
        /// 空值
        /// </summary>
        NULL = 0,

        /// <summary>
        /// HTTP GET方法
        /// </summary>
        GET = 2,

        /// <summary>
        /// HTTP POST方法
        /// </summary>
        POST = 4,

        /// <summary>
        /// HTTP PUT方法
        /// </summary>
        PUT = 8,

        /// <summary>
        /// HTTP DELETE方法
        /// </summary>
        DELETE = 16,

        /// <summary>
        /// HTTP HEAD方法
        /// </summary>
        HEAD = 32
    }

    public static class HttpVerbExts
    {
        /// <summary>
        /// 将 HttpMethod 字符转换到等效的 HttpVerb 枚举
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static HttpVerb ToHttpVerb(this string method)
        {
            switch (method)
            {
                case "GET": 
                    return HttpVerb.GET;
                case "POST": 
                    return HttpVerb.POST;
                case "PUT": 
                    return HttpVerb.PUT;
                case "DELETE": 
                    return HttpVerb.DELETE;
                case "HEAD": 
                    return HttpVerb.HEAD;
                default: 
                    return HttpVerb.NULL;
            }
        }
    }
}
