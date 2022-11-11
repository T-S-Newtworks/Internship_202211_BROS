using System.Web;
using System.Web.Mvc;
using Internship_Template.Common;


namespace Internship_Template
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // ログイン認証
            filters.Add(new LoginFilter());

        }
    }
}
