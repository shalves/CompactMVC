namespace System.Web
{
    /// <summary>
    /// Action的HttpMethod标记类
    /// <para>用于限制Action执行特定的请求，未添加此标记则允许所有请求类型</para>
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
        /// 当请求类型不匹配时，请指定重定向的目标Url
        /// </summary>
        public string RedirectUrl { get; set; }

        public override bool IsFatalError
        {
            get { return !string.IsNullOrEmpty(RedirectUrl); }
        }

        /// <summary>
        /// 通过指定多个谓词来标记Action可接受的Http请求的类型
        /// </summary>
        /// <param name="allow"></param>
        public HttpMethodAttribute(HttpVerb allow)
        {
            _Allow = allow;
        }

        /// <summary>
        /// 验证Action设定的HttpMethod标记
        /// </summary>
        /// <param name="controler"></param>
        /// <returns></returns>
        public override bool Validate(Controler controler)
        {
            return this.Allow == (this.Allow | controler.HttpMethod);
        }
    }
}
