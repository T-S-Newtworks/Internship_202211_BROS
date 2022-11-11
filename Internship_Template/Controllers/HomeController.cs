using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Internship_Template.Models.Entity;

namespace Internship_Template.Controllers
{
    /// <summary>
    /// Top画面のコントローラです。
    /// </summary>
    /// <remarks>Top画面に機能を追加したい場合はここに処理を追加しましょう。</remarks>
    public class HomeController : Controller
    {

        private Internship_202211 db = new Internship_202211();
        public ActionResult Index()
        {
            List<T_USER> users = db.T_USER.ToList() ?? new List<T_USER>();

            return View(users);
        }

    }
}