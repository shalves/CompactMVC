using System.Reflection;

namespace System.Web
{
    /// <summary>
    /// 为控制器和操作方法提供认证标记设置
    /// <para>用于限制只有特定的认证用户才可以请求被标记的控制器或操作方法</para>
    /// </summary>
    public sealed class AuthenticationAttribute : AttributeBase
    {
        string[] _Roles = new string[] { "*" };

        public override bool IsFatalError
        {
            get { return true; }
        }

        /// <summary>
        /// 表示被标记的控制器或操作方法允许的用户角色类型的数组
        /// </summary>
        public string[] Roles 
        {
            get { return _Roles; }
        }

        /// <summary>
        /// 声明只有经过认证的用户才可以请求该控制器或操作方法
        /// </summary>
        public AuthenticationAttribute() { }

        /// <summary>
        /// 声明只有经过认证的用户才可以请求该控制器或操作方法
        /// </summary>
        /// <param name="roles">指定控制器或操作方法允许的用户角色类型的数组</param>
        public AuthenticationAttribute(params string[] roles)
        {
            this._Roles = roles;
        }

        /// <summary>
        /// 使用该Authentication标记设置验证当前HTTP 请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool Validate(HttpContextBase context)
        {
            if (context.Request.IsAuthenticated)
            {
                if (this.Roles[0].Equals("*")) return true;
                foreach (var r in this.Roles)
                {
                    if (context.User.IsInRole(r))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
