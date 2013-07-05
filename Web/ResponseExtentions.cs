namespace System.Web
{
    public static class ResponseExtentions
    {
        /// <summary>
        /// 发送301响应（永久性重定向）到客户端
        /// </summary>
        /// <param name="targetUrl">目标Url</param>
        public static void Send301(this HttpResponseBase Response, string url)
        {
            Response.StatusCode = 301;
            Response.StatusDescription = "Moved Permanently";
            Response.AppendHeader("Location", url);
            Response.End();
        }

        /// <summary>
        /// 发送401响应（请求未授权）到客户端
        /// </summary>
        /// <param name="Response"></param>
        public static void Send401(this HttpResponseBase Response)
        {
            Response.StatusCode = 401;
            Response.StatusDescription = "Unauthorized";
            Response.End();
        }

        /// <summary>
        /// 发送403响应（禁止访问）到客户端
        /// </summary>
        /// <param name="Response"></param>
        public static void Send403(this HttpResponseBase Response)
        {
            Response.StatusCode = 403;
            Response.StatusDescription = "Forbidden";
            Response.End();
        }

        /// <summary>
        /// 发送404响应（资源不存在）到客户端
        /// </summary>
        /// <param name="Response"></param>
        public static void Send404(this HttpResponseBase Response)
        {
            Response.StatusCode = 404;
            Response.StatusDescription = "Not Found";
            Response.End();
        }
    }
}
