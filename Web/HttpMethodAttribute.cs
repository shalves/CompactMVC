namespace System.Web
{
    /// <summary>
    /// 为控制器和操作方法提供 HTTP 请求类型标记设置
    /// <para>用于限制只有特定的请求类型才可以请求被标记的控制器或操作方法</para>
    /// </summary>
    public sealed class HttpMethodAttribute : AttributeBase
    {
        HttpVerb _Allow = HttpVerb.GET | HttpVerb.POST;

        public override bool IsFatalError
        {
            get { return !string.IsNullOrEmpty(RedirectUrl); }
        }

        /// <summary>
        /// 获取被标记的控制器或操作方法可接受的 HTTP 请求的类型
        /// </summary>
        public HttpVerb Allow
        {
            get { return _Allow; }
        }

        /// <summary>
        /// 当请求类型不匹配时，指定重定向的目标 URL
        /// <para>将导致 HTTP GET 重定向</para>
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 当指定了重定向的目标 URL 时，指定是否保留原始 URL 中的 QueryString 部分
        /// </summary>
        public bool ReserveQueryString { get; set; }

        /// <summary>
        /// 通过指定多个谓词来限制控制器或操作方法可接受的 HTTP 请求的类型
        /// </summary>
        /// <param name="allow"></param>
        public HttpMethodAttribute(HttpVerb allow)
        {
            this._Allow = allow;
        }

        /// <summary>
        /// 使用该 HttpMethod 标记设置验证当前 HTTP 请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool Validate(HttpContextBase context)
        {
            return Allow == (Allow | context.Request.HttpMethod.ToHttpVerb());
        }
    }
}
