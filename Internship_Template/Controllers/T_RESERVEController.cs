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
    public class T_RESERVEController : Controller
    {
        private Internship_202211 db = new Internship_202211();

        // GET: T_RESERVE
        public ActionResult Index()
        {
            //Sessionを呼び出す
            T_USER loginUser = (T_USER)Session[M_SESSION.SessionKey];
            //ユーザーIDを決め打ち(暫定)
            string id = loginUser.ID;

            var t_RESERVE = db.T_RESERVE.Include(t => t.T_USER).Include(t => t.T_POINT).Include(t => t.T_POINTEND).Include(t => t.T_DATEANDTIME).Where(e => e.T_USER.ID == id);
            return View(t_RESERVE.ToList());
        }

        // GET: T_RESERVE/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_RESERVE t_RESERVE = db.T_RESERVE.Find(id);
            if (t_RESERVE == null)
            {
                return HttpNotFound();
            }
            return View(t_RESERVE);
        }

        // GET: T_RESERVE/Create
        public ActionResult Create(String DateandtimeId, String Direction)
        {

            var user = (T_USER)Session[M_SESSION.SessionKey];

           ViewBag.Payment = user.PAYMENTID;

            //Directionが上りか下りか☑

            ViewBag.USERID = new SelectList(db.T_USER, "ID", "FULLNAME");
            //T_DATEANDTIMEの一覧を取ってくる（重複含む）
            List<T_DATEANDTIME> timeList1 = db.T_DATEANDTIME.OrderBy(a => a.YMD).ToList();
            List<T_DATEANDTIME> sortTimeList = new List<T_DATEANDTIME>();
            //上りならViewBag.STARTPOINTIDには日立と多賀のデータだけ入れるViewBag.ENDPOINTIDには上野と東京のみ
            if (Direction == "0")
            {
                foreach (var item in timeList1)
                {
                    if(item.T_TIMETABLE.DIRECTION == "0")
                    {
                        var temp = item.YMD;
                        string displayYMD = temp.Value.ToString("yyyy/MM/dd");
                        item.YMDString = displayYMD + item.T_TIMETABLE.HITACHI_TIME;

                        sortTimeList.Add(item);


                    }
                }
                ViewBag.STARTPOINTID = new SelectList(db.T_POINT.Where(e => e.ID == "1" || e.ID == "2"), "ID", "NAME");


                ViewBag.ENDPOINTID = new SelectList(db.T_POINT.Where(e => e.ID == "3" || e.ID == "4"), "ID", "NAME");
            }
            //下りなら逆パターン
            else
            {
                foreach (var item in timeList1)
                {
                    if (item.T_TIMETABLE.DIRECTION == "1")
                    {
                        var temp = item.YMD;
                        string displayYMD = temp.Value.ToString("yyyy/MM/dd");
                        item.YMDString = displayYMD + item.T_TIMETABLE.TOKYO_TIME;

                        sortTimeList.Add(item);


                    }
                }

                    ViewBag.STARTPOINTID = new SelectList(db.T_POINT.Where(e => e.ID == "3" || e.ID == "4"), "ID", "NAME");

                ViewBag.ENDPOINTID = new SelectList(db.T_POINT.Where(e => e.ID == "1" || e.ID == "2"), "ID", "NAME");
            }

            T_DATEANDTIME targetReserve = sortTimeList.Where(e => e.ID == DateandtimeId).FirstOrDefault();
            sortTimeList.Remove(targetReserve);
            sortTimeList.Insert(0, targetReserve);

            //変換したやつを下の処理に突っ込む
            ViewBag.DATEANDTIMEID = new SelectList(sortTimeList, "ID", "YMDString");
            ViewBag.Direction = Direction;
            //シートの列のデータリストを作成
            List<SelectListItem> Seat = new List<SelectListItem>();
                Seat.Add(new SelectListItem
                {
                    Text = "A",
                    Value = "A",
                });
                Seat.Add(new SelectListItem
                {
                    Text = "B",
                    Value = "B",
                });
                Seat.Add(new SelectListItem
                {
                    Text = "C",
                    Value = "C",
                });

            //シートの番号のデータリストを作成
            List<SelectListItem> Seat_num = new List<SelectListItem>();
            for(int i=1; i<8; i++)
            {
                Seat_num.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                });
            }


            //列のデータをドロップダウンリスト用の変数に埋め込む
            ViewBag.Seat = new SelectList(Seat, "Value", "Text");
            ViewBag.Seat_num = new SelectList(Seat_num, "Value", "Text");


            var model = new T_RESERVE();
            model.T_DATEANDTIME = db.T_DATEANDTIME.Find(DateandtimeId);
            model.PRICE = 2500;
            return View(model);
        }

        // POST: T_RESERVE/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DATEANDTIMEID,STARTPOINTID,ENDPOINTID,SEAT,PRICE,USERID,ISCANCEL")] T_RESERVE t_RESERVE, string Seat, string Seat_num, string paymentid)
        {
            if (ModelState.IsValid)
            {
                //Seatの設定
                t_RESERVE.SEAT = Seat + "-" + Seat_num;

                //IDの設定
                var count = db.T_RESERVE.Count();
                t_RESERVE.ID = (count + 1).ToString();

                //ユーザIDの設定
                var user = (T_USER)Session[M_SESSION.SessionKey];
                t_RESERVE.USERID = user.ID;

                //支払い方法の設定(デフォルトでクレジットカードを想定)
                paymentid = "1";

                db.T_RESERVE.Add(t_RESERVE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.USERID = new SelectList(db.T_USER, "ID", "FULLNAME", t_RESERVE.USERID);
            ViewBag.STARTPOINTID = new SelectList(db.T_POINT, "ID", "NAME", t_RESERVE.STARTPOINTID);
            ViewBag.ENDPOINTID = new SelectList(db.T_POINT, "ID", "NAME", t_RESERVE.ENDPOINTID);
            ViewBag.DATEANDTIMEID = new SelectList(db.T_DATEANDTIME, "ID", "TIMETABLEID", t_RESERVE.DATEANDTIMEID);
            return View(t_RESERVE);
        }

        // GET: T_RESERVE/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_RESERVE t_RESERVE = db.T_RESERVE.Find(id);
            if (t_RESERVE == null)
            {
                return HttpNotFound();
            }
            ViewBag.USERID = new SelectList(db.T_USER, "ID", "FULLNAME", t_RESERVE.USERID);
            ViewBag.STARTPOINTID = new SelectList(db.T_POINT, "ID", "NAME", t_RESERVE.STARTPOINTID);
            ViewBag.ENDPOINTID = new SelectList(db.T_POINT, "ID", "NAME", t_RESERVE.ENDPOINTID);
            ViewBag.DATEANDTIMEID = new SelectList(db.T_DATEANDTIME, "ID", "TIMETABLEID", t_RESERVE.DATEANDTIMEID);
            return View(t_RESERVE);
        }

        // POST: T_RESERVE/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DATEANDTIMEID,STARTPOINTID,ENDPOINTID,SEAT,PRICE,USERID,ISCANCEL")] T_RESERVE t_RESERVE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_RESERVE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.USERID = new SelectList(db.T_USER, "ID", "FULLNAME", t_RESERVE.USERID);
            ViewBag.STARTPOINTID = new SelectList(db.T_POINT, "ID", "NAME", t_RESERVE.STARTPOINTID);
            ViewBag.ENDPOINTID = new SelectList(db.T_POINT, "ID", "NAME", t_RESERVE.ENDPOINTID);
            ViewBag.DATEANDTIMEID = new SelectList(db.T_DATEANDTIME, "ID", "TIMETABLEID", t_RESERVE.DATEANDTIMEID);
            return View(t_RESERVE);
        }

        // GET: T_RESERVE/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_RESERVE t_RESERVE = db.T_RESERVE.Find(id);
            if (t_RESERVE == null)
            {
                return HttpNotFound();
            }
            return View(t_RESERVE);
        }

        // POST: T_RESERVE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            T_RESERVE t_RESERVE = db.T_RESERVE.Find(id);
            //t_RESERVE.ISCANCEL = "1";
            //更新
            //db.Entry(t_RESERVE);
            //db.SaveChanges();

            //削除
            db.T_RESERVE.Remove(t_RESERVE);
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

        public ActionResult Cancel (string id)
        {
            T_RESERVE user = db.T_RESERVE.Find(id);
            if (user.ISCANCEL == null)
            {
                user.ISCANCEL = "1"; //キャンセルの場合ISCANCELに"1"が入ります
                db.Entry(user).State = EntityState.Modified; //userのエンティティが変更されています
                db.SaveChanges(); //Modifiedのエンティティがデータベースで更新されます

            }
            //再検索する
            
            return RedirectToAction("Index");
        }
    }
}
