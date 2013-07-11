namespace System.Web
{
    /// <summary>
    /// 操作方法（Action）的 HttpMethod 标记类
    /// <para>用于限制只有特定的请求类型才可以请求被标记的操作方法</para>
    /// </summary>
    public sealed class HttpMethodAttribute : ActionAttribute
    {
        HttpVerb _Allow = HttpVerb.GET | HttpVerb.POST;

        /// <summary>
        /// 获取被标记操作方法可接受的Http请求的类型
        /// </summary>
        public HttpVerb Allow
        {
            get { return _Allow; }
        }

        /// <summary>
        /// 当请求类型不匹配时，指定重定向的目标Url
        /// <para>HTTP GET 重定向</para>
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 当指定了重定向的目标Url时，指定是否保留原始Url中的QueryString部分
        /// </summary>
        public bool ReserveQueryString { get; set; }

        public override bool IsFatalError
        {
            get { return !string.IsNullOrEmpty(RedirectUrl); }
        }

        /// <summary>
        /// 通过指定多个谓词来控制操作方法可接受的Http请求的类型
        /// </summary>
        /// <param name="allow"></param>
        public HttpMethodAttribute(HttpVerb allow)
        {
            this._Allow = allow;
        }

        /// <summary>
        /// 使用该HttpMethod标记设置验证当前Http请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool Validate(HttpContextBase context)
        {
            return Allow == (Allow | context.Request.HttpMethod.ToHttpVerb());
        }
    }
}
