using System.Reflection;

namespace System.Web
{
    /// <summary>
    /// Action的Authentication标记类
    /// <para>被标记的方法限制是否认证用户才可以请求被标记的Action</para>
    /// </summary>
    public sealed class AuthenticationAttribute : ActionAttribute
    {
        string[] _Roles = new string[] { "*" };

        /// <summary>
        /// 表示被标记的方法能够接受的用户角色类型的数组
        /// </summary>
        public string[] Roles 
        {
            get { return _Roles; }
        }

        public override bool IsFatalError
        {
            get { return true; }
        }

        /// <summary>
        /// 声明只有认证的用户才可以请求该Action
        /// </summary>
        public AuthenticationAttribute() { }

        /// <summary>
        /// 声明只有指定角色的认证用户才可以请求该Action
        /// </summary>
        /// <param name="roles"></param>
        public AuthenticationAttribute(params string[] roles)
        {
            _Roles = roles;
        }

        /// <summary>
        /// 验证Action的Authentication标记
        /// </summary>
        /// <param name="controler"></param>
        /// <returns></returns>
        public override bool Validate(Controler controler)
        {
            if (controler.Request.IsAuthenticated)
            {
                if (this.Roles[0].Equals("*")) return true;
                foreach (var r in this.Roles)
                {
                    if (controler.User.IsInRole(r))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
