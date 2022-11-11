using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Internship_Template.Models.Entity;

namespace Internship_Template.Controllers
{
    public class T_DATEANDTIMEController : Controller
    {
        private Internship_202211 db = new Internship_202211();

        // GET: T_DATEANDTIME
        public ActionResult Index()
        {
            
            var t_DATEANDTIME = db.T_DATEANDTIME.Include(t => t.T_TIMETABLE);
            
            return View(new List<T_DATEANDTIME>()/*t_DATEANDTIME.ToList()*/);
        }

        public ActionResult Search(string direction, string ymd)
        {
            //検索結果格納用オブジェクト
            List<T_DATEANDTIME> t_dateandtime = new List<T_DATEANDTIME>();

            //方向検索

            if (!string.IsNullOrEmpty(direction) && direction != "選択して下さい")
            {                
                //日付検索
                if (!string.IsNullOrEmpty(ymd))
                {
                    //方向検索
                    t_dateandtime = db.T_DATEANDTIME.Where(e => e.T_TIMETABLE.DIRECTION.Contains(direction)).ToList();
                    //検索
                    string format = "yyyy-MM-dd";
                    DateTime dTime = DateTime.ParseExact(ymd, format, null);
                    t_dateandtime = t_dateandtime.Where(e => e.YMD == dTime).ToList();
                    //完全一致は下のように書く
                    //targetUsers = _db.T_USER.Where(e => e.FULLNAME == userName).ToList();
                    if(direction == "0") {
                        t_dateandtime = t_dateandtime.OrderBy(e => e.T_TIMETABLE.HITACHI_TIME).ToList();
                    }else if(direction == "1")
                    {
                        t_dateandtime = t_dateandtime.OrderBy(e => e.T_TIMETABLE.TOKYO_TIME).ToList();

                    }

                }
                else
                {
                    //日付が未入力ならば空のリストを返す
                    t_dateandtime = new List<T_DATEANDTIME>();
                    //HttpContext.Session.RemoveAll();
                    ViewBag.ErrorText = "正しい値を入力して下さい。";
                }
            }
            else
            {
                //「選択してください」ならば空のリストを返す
                t_dateandtime = new List<T_DATEANDTIME>();
                //HttpContext.Session.RemoveAll();
                ViewBag.ErrorText = "正しい値を入力して下さい。";
            }

            //ドロップダウンリスト復元
            ViewBag.SelectDirection = direction;
            ViewBag.SelectYMD = ymd;
            //TempData["direction"] = direction;
            //TempData["ymd"] = ymd;
            return View("Index", t_dateandtime);
        }
        public ActionResult Error()
        {
            //セッション情報を破棄する。
            HttpContext.Session.RemoveAll();
            ViewBag.ErrorText = "";
            return Index();
        }
        // GET: T_DATEANDTIME/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_DATEANDTIME t_DATEANDTIME = db.T_DATEANDTIME.Find(id);
            if (t_DATEANDTIME == null)
            {
                return HttpNotFound();
            }
            return View(t_DATEANDTIME);
        }

        // GET: T_DATEANDTIME/Create
        public ActionResult Create()
        {
            ViewBag.TIMETABLEID = new SelectList(db.T_TIMETABLE, "ID", "DIRECTION");
            return View();
        }

        // POST: T_DATEANDTIME/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TIMETABLEID,YMD")] T_DATEANDTIME t_DATEANDTIME)
        {
            if (ModelState.IsValid)
            {
                db.T_DATEANDTIME.Add(t_DATEANDTIME);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TIMETABLEID = new SelectList(db.T_TIMETABLE, "ID", "DIRECTION", t_DATEANDTIME.TIMETABLEID);
            return View(t_DATEANDTIME);
        }

        // GET: T_DATEANDTIME/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_DATEANDTIME t_DATEANDTIME = db.T_DATEANDTIME.Find(id);
            if (t_DATEANDTIME == null)
            {
                return HttpNotFound();
            }
            ViewBag.TIMETABLEID = new SelectList(db.T_TIMETABLE, "ID", "DIRECTION", t_DATEANDTIME.TIMETABLEID);
            return View(t_DATEANDTIME);
        }

        // POST: T_DATEANDTIME/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TIMETABLEID,YMD")] T_DATEANDTIME t_DATEANDTIME)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_DATEANDTIME).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TIMETABLEID = new SelectList(db.T_TIMETABLE, "ID", "DIRECTION", t_DATEANDTIME.TIMETABLEID);
            return View(t_DATEANDTIME);
        }

        // GET: T_DATEANDTIME/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_DATEANDTIME t_DATEANDTIME = db.T_DATEANDTIME.Find(id);
            if (t_DATEANDTIME == null)
            {
                return HttpNotFound();
            }
            return View(t_DATEANDTIME);
        }

        // POST: T_DATEANDTIME/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            T_DATEANDTIME t_DATEANDTIME = db.T_DATEANDTIME.Find(id);
            db.T_DATEANDTIME.Remove(t_DATEANDTIME);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
