namespace System.Web
{
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// 使 ASP.NET 跳过 HTTP 执行管线链中的所有事件和筛选并直接执行 EndRequest 事件
        /// <para>不会引发“ThreadAbortException”，用于替代Response.End（）方法</para>
        /// </summary>
        public static void BetterEnd(this HttpResponse response)
        {
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// 将客户端重定向到新的 URL 并指定该新 URL。
        /// <para>不会引发“ThreadAbortException”，用于替代Response.Redirect（url）方法</para>
        /// </summary>
        /// <param name="url">目标的位置。</param>
        public static void BetterRedirect(this HttpResponse response, string url)
        {
            response.Redirect(url, false);
            response.BetterEnd();
        }

        /// <summary>
        /// 将客户端重定向到新的 URL 指定该新 URL 并指定当前页的执行是否终止。
        /// <para>不会引发“ThreadAbortException”，用于替代Response.Redirect（url）方法</para>
        /// </summary>
        /// <param name="url">目标的位置。</param>
        /// <param name="endResponse">指示当前页的执行是否应终止。</param>
        public static void BetterRedirect(this HttpResponse response, string url, bool endResponse)
        {
            response.Redirect(url, false);
            if (endResponse) response.BetterEnd();
        }
    }
}
