using System.Reflection;

namespace System
{
    public static class PatchVersion
    {
        readonly static string _Value;
        /// <summary>
        /// 获取System(Patch)程序集的版本号
        /// </summary>
        public static string Value
        {
            get
            {
                return _Value;
            }
        }

        static PatchVersion()
        {
            _Value = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
