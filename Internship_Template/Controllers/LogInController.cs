using System.Web.Mvc;
using Internship_Template.Models.Entity;


namespace Internship_Template.Controllers
{
    public class LogInController : Controller
    {

        private Internship_202211 db = new Internship_202211();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            //ExecuteLoginやExecuteLogoutもこのアクション利用するのでViewNameを指定する必要がある。
            return View("Index");
        }

        /// <summary>
        /// ログインの実行
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExecuteLogin(T_LOGIN model)
        {
            if(string.IsNullOrEmpty(model.ID) || string.IsNullOrEmpty(model.PASSWORD))
            {
                // ユーザー認証 失敗
                this.ModelState.AddModelError("LoginError", "IDおよびパスワードは必須です。");
                //↑のメッセージを引き継ぎたいのでRedirectToActionを行わない。Redirectする場合はTempDataで引き継ぐ必要がある。
                return Index();

            }

            T_LOGIN loginInfo = db.T_LOGIN.Find(model.ID);
            if (loginInfo != null && loginInfo.PASSWORD == model.PASSWORD)
            {
                // ユーザー認証 成功
                //LoginInfoをもとにユーザー情報を取得
                T_USER loginUser = db.T_USER.Find(loginInfo.ID);
                HttpContext.Session[M_SESSION.SessionKey] = loginUser;
                return this.Redirect("/");
            }
            else
            {
                // ユーザー認証 失敗
                this.ModelState.AddModelError("LoginError", "指定されたユーザー名またはパスワードが正しくありません。");
                return Index();
            }
        }

        /// <summary>
        /// ログアウト実行
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExecuteLogout()
        {
            //セッション情報を破棄する。
            HttpContext.Session.RemoveAll();
            ViewBag.LogoutText = "ログアウトしました。利用する際は再度ログインしてください。";
            return Index();
        }

    }

}