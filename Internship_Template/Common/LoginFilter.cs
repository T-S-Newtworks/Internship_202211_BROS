using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Internship_Template.Models.Entity;

namespace Internship_Template.Common
{
    /// <summary>
    /// ログイン認証フィルター
    /// </summary>
    public class LoginFilter : FilterAttribute, IAuthorizationFilter
    {

        /// <summary>
        /// 認証処理
        /// </summary>
        /// <param name="filterContext"></param>
        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {

            //セッション内容の取得
            HttpContextBase httpContext = filterContext.HttpContext;
            string debugMode = Internship_Util.GetValue<string>("DebugMode");
            //セッションが生成されるのはログイン画面を経由した場合のみ
            if (httpContext.Session[M_SESSION.SessionKey] is T_USER)
            {
                return;
            }
            else if(debugMode == "Admin")
            {
                //田中太郎固定でセッション作成
                Internship_202211 db = new Internship_202211();
                T_USER admin = db.T_USER.Where(e => e.ID == "1").FirstOrDefault();
                httpContext.Session[M_SESSION.SessionKey] = admin;

                return;
            }
            else
            {
                object controllerName = "Login";
                RouteData currentRoute = filterContext.RequestContext.RouteData;

                /*TODO: 改善の余地あり。今の作りだと{controller}{action}{id}のいずれかにLoginがあるとLoginControllerに飛んでしまう。
                        あと直でmodel渡された場合も侵入できてしまうから何とかする。*/
                if (currentRoute.Values.ContainsValue(controllerName))
                {
                    return;
                }
                else
                {
                    //ユーザーをログイン画面にリダイレクトします  
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                             { "controller", "Login" },
                             { "action", "Index" }
                        });
                }
            }


        }
    }
}