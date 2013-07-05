namespace System.Web
{
    /// <summary>
    /// Action的HttpMethod标记类
    /// <para>用于限制只有特定的请求类型才可以请求被标记的Action</para>
    /// </summary>
    public sealed class HttpMethodAttribute : ActionAttribute
    {
        HttpVerb _Allow = HttpVerb.GET | HttpVerb.POST;

        /// <summary>
        /// 获取被标记Action可接受的Http请求的类型
        /// </summary>
        public HttpVerb Allow
        {
            get { return _Allow; }
        }

        /// <summary>
        /// 当请求类型不匹配时，指定重定向的目标Url
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 当指定了重定的向目标Url时，指定是否保留原始Url中的QueryString部分
        /// </summary>
        public bool PreserveQueryString { get; set; }

        public override bool IsFatalError
        {
            get { return !string.IsNullOrEmpty(RedirectUrl); }
        }

        /// <summary>
        /// 通过指定多个谓词来控制Action可接受的Http请求的类型
        /// </summary>
        /// <param name="allow"></param>
        public HttpMethodAttribute(HttpVerb allow)
        {
            _Allow = allow;
        }

        /// <summary>
        /// 验证Action设定的HttpMethod标记
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool Validate(HttpContextBase context)
        {
            return this.Allow == (this.Allow | context.Request.HttpMethod.ToHttpVerb());
        }
    }
}
