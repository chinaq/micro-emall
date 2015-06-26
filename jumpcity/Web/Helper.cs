using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Collections.Generic;
using Jumpcity.Utility.Extend;

namespace Jumpcity.Web
{
    /// <summary>
    /// 提供Web页面操作的通用工具方法和属性
    /// </summary>
    public static class Helper
    {
        private static string _adminBase = "/admin/modules";

        static Helper()
        {
            string adminBase = ConfigurationManager.AppSettings["adminBase"];
            if (!General.IsNullable(adminBase))
                _adminBase = adminBase;
        }

        /// <summary>
        /// 获取或设置当前登陆的用户信息，该信息会被储存在Session中
        /// </summary>
        public static Account User
        {
            get { return HttpContext.Current.Session["user"] as Account; }
            set { HttpContext.Current.Session["user"] = value; }
        }

        /// <summary>
        /// 获取或设置当前登陆的管理员信息，该信息会被储存在Session中
        /// </summary>
        public static Account Admin
        {
            get { return HttpContext.Current.Session["admin"] as Account; }
            set { HttpContext.Current.Session["admin"] = value; }
        }

        /// <summary>
        /// 获取或设置当前所在的模块信息
        /// </summary>
        public static string Module
        {
            get
            {
                string mod = HttpContext.Current.Session["module"] as string;
                return mod != null ? mod : string.Empty;
            }
            set { HttpContext.Current.Session["module"] = value; }
        }

        /// <summary>
        /// 获取或设置当前的执行动作
        /// </summary>
        public static string Action
        {
            get
            {
                string action = HttpContext.Current.Session["action"] as string;
                return action != null ? action : string.Empty;
            }
            set { HttpContext.Current.Session["action"] = value; }
        }

        /// <summary>
        /// 执行客户端方法
        /// </summary>
        /// <param name="script">要执行的客户端代码片断</param>
        /// <param name="key">执行内容的Key</param>
        public static void ExecScript(string script, string key = null)
        {
            Page p = HttpContext.Current.CurrentHandler as Page;
            key = General.IsNullable(key) ? General.UniqueString() : key;

            if (!General.IsNullable(script) && p != null)
            {
                p.ClientScript.RegisterStartupScript(
                    p.GetType(),
                    key,
                    script,
                    true
                );
            }
        }

        /// <summary>
        /// 用指定的参数调用客户端方法在顶部显示一个操作状态提示框
        /// </summary>
        /// <param name="isSuccess">指示操作是否成功</param>
        /// <param name="message">要显示的文本内容</param>
        public static void MessageBox(bool isSuccess = true, string message = "")
        {
            if (General.IsNullable(message))
                message = "操作" + (isSuccess ? "成功" : "失败");

            ExecScript(
                string.Format("var __message = '{0}'; var __success = {1};", message, isSuccess ? 1 : 0)
            );
        }

        /// <summary>
        /// 根据当前的模块名和动作名来更新导航菜单的选中状态
        /// </summary>
        /// <param name="module">当前的模块名</param>
        /// <param name="action">当前的动作名</param>
        public static void Active(string module = null, string action = null)
        {
            module = (module == null ? Module : module);
            action = (action == null ? Action : action);
            string script = string.Format("var __module = '{0}';var __action = '{1}';", module, action);
            ExecScript(script);
        }

        /// <summary>
        /// 根据设置的模块名、动作名以及参数列表组合成一个可以访问的后台数据中心URL地址
        /// </summary>
        /// <param name="module">模块名称(对应Web应用的文件夹名称)</param>
        /// <param name="action">动作名称(对应Web应用的ASPX文件名称)</param>
        /// <param name="parameters">要附加的参数列表，序列化后的字符串格式为"name=value"</param>
        /// <returns>如果参数都正确，返回组合好的可访问的后台数据中心URL地址，否则返回空字符串</returns>
        public static string Link(string module, string action, Dictionary<string, string> parameters = null)
        {
            string[] pArray = null;
            if (!General.IsNullable(parameters))
            {
                int i = 0;
                pArray = new string[parameters.Count];
                foreach(var item in parameters)
                {
                    pArray[i] = item.Key + "=" + item.Value;
                    i++;
                }
            }

            return Link(_adminBase, module, action, pArray);
        }

        /// <summary>
        /// 根据当前活动的模块名、动作名以及指定的参数列表组合成一个可以访问的后台数据中心URL地址
        /// </summary>
        /// <param name="parameters">要附加的参数列表，字符串格式为"name=value"</param>
        /// <returns>如果参数都正确，返回组合好的可访问的后台数据中心URL地址，否则返回空字符串</returns>
        public static string Link(params string[] parameters)
        {
            return Link(Module, Action, parameters);
        }

        /// <summary>
        /// 根据设置的模块名、动作名以及参数列表组合成一个可以访问的后台数据中心URL地址
        /// </summary>
        /// <param name="module">模块名称(对应Web应用的文件夹名称)</param>
        /// <param name="action">动作名称(对应Web应用的ASPX文件名称)</param>
        /// <param name="parameters">要附加的参数列表，字符串格式为"name=value"</param>
        /// <returns>如果参数都正确，返回组合好的可访问的后台数据中心URL地址，否则返回空字符串</returns>
        public static string Link(string module, string action, params string[] parameters)
        {
            return Link(_adminBase, module, action, parameters);
        }

        /// <summary>
        /// 根据设置的基地址、模块名、动作名以及参数列表组合成一个可以访问的URL地址
        /// </summary>
        /// <param name="basePath">基地址路径字符串</param>
        /// <param name="module">模块名称(对应Web应用的文件夹名称)</param>
        /// <param name="action">动作名称(对应Web应用的ASPX文件名称)</param>
        /// <param name="parameters">要附加的参数列表，字符串格式为"name=value"</param>
        /// <returns>如果参数都正确，返回组合好的可访问URL地址，否则返回空字符串</returns>
        public static string Link(string basePath, string module, string action, params string[] parameters)
        {
            string link = string.Empty;

            if (!General.IsNullable(basePath) && !General.IsNullable(module) && !General.IsNullable(action))
            {
                link = string.Format("{0}/{1}/{2}.aspx?m={1}&a={2}", basePath, module, action);

                if (!General.IsNullable(parameters))
                    link += "&" + string.Join("&", parameters);
            }

            return link;
        }

        /// <summary>
        /// 提交至新页面之前确定当前模块和当前动作，该方法一般在Global.asax文件中使用
        /// </summary>
        public static void ApplicationAcquireRequestState()
        {
            string module = HttpContext.Current.Request.GetString("m");
            string action = HttpContext.Current.Request.GetString("a");

            if (!General.IsNullable(module))
                Helper.Module = module;

            if (!General.IsNullable(action))
                Helper.Action = action;
        }

        /// <summary>
        /// 在新会话启动时运行的初始化Session项的代码，该方法一般在Global.asax文件中使用
        /// </summary>
        public static void SessionStart()
        {       
            HttpContext.Current.Session["module"] = null;
            HttpContext.Current.Session["action"] = null;
            HttpContext.Current.Session["user"] = null;
            HttpContext.Current.Session["admin"] = null;
        }
    }
}
