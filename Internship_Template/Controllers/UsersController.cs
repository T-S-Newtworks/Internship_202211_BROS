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
    public class UsersController : Controller
    {

        private Internship_Context _db = new Internship_Context();

        /// <summary>
        /// 初期画面表示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<T_USER> users = _db.T_USER.ToList() ?? new List<T_USER>();
            //ドロップダウンリスト生成
            ViewBag.AdminDropDown = CreateAdminDropDown(true);

            //登録更新削除時の結果メッセージあるならViewBagに代入
            if (TempData["resultText"] != null)
                ViewBag.ResultText = TempData["resultText"];
            if (TempData["isSuccess"] != null)
                ViewBag.isSuccess = TempData["isSuccess"];
            return View(users);

        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="userName">ユーザ名入力値</param>
        /// <param name="admin">権限選択値</param>
        /// <returns></returns>
        public ActionResult Search(string userName, string admin)
        {
            //検索結果格納用オブジェクト
            List<T_USER> targetUsers = new List<T_USER>();

            //氏名検索
            if (!string.IsNullOrEmpty(userName))
            {
                //あいまい検索
                targetUsers = _db.T_USER.Where(e => e.FULLNAME.Contains(userName)).ToList();
                //完全一致は下のように書く
                //targetUsers = _db.T_USER.Where(e => e.FULLNAME == userName).ToList();
            }
            else
            {
                //未入力ならば全件返す
                targetUsers = _db.T_USER.ToList();
            }

            //権限検索
            if (!string.IsNullOrEmpty(admin))
            {
                //権限は完全一致のみ
                targetUsers = targetUsers.Where(e => e.ADMINFLG == admin).ToList();
            }
            else
            {
                //選択してくださいならば全件返す
                targetUsers = targetUsers.ToList();
            }

            //ドロップダウンリスト復元
            ViewBag.AdminDropDown = CreateAdminDropDown(true);

            return View("Index", targetUsers);
        }
        /// <summary>
        /// プロフィール画面表示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_USER targetUser = _db.T_USER.Find(id);
            if(targetUser == null)
            {
                return HttpNotFound();
            }
            return View(targetUser);

        }

        /// <summary>
        /// ユーザー追加画面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            //ドロップダウンリスト生成
            ViewBag.AdminDropDown = CreateAdminDropDown();
            return View();
        }

        /// <summary>
        /// ユーザー追加画面
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateComplete (T_USER user, HttpPostedFileBase uploadImage = null)
        {
            if (uploadImage.ContentType.StartsWith("image/"))
            {
                byte[] data = new Byte[uploadImage.ContentLength];
                uploadImage.InputStream.Read(data, 0, uploadImage.ContentLength);
                string mimeType = uploadImage.ContentType;
                user.ICON = data;  // データ本体
                user.MIMETYPE = mimeType;
                user.T_LOGIN.ID = user.ID;

                // エンティティを追加＆データソースに反映
                //画像追加しない場合はこの２行のみ記述してif文の処理は無視してもらって大丈夫
                //TODO: ほんとはTryCatchしたいかな。スキャフォールディングにはないので実装しません。
                _db.T_USER.Add(user);
                _db.SaveChanges();

                //成功メッセージくらいは出す。
                TempData["resultText"] = "登録成功!";

            }
            else
            {
                // 画像ファイルでない場合はエラー
                ViewData["msg"] = "画像以外はアップロードできません。";
                return View("Create", user);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 更新画面表示
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_USER targetUser = _db.T_USER.Find(id);
            if (targetUser == null)
            {
                return HttpNotFound();
            }
                return View(targetUser);
        }

        /// <summary>
        /// 更新完了時
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComplete(T_USER model, HttpPostedFileBase uploadImage = null)
        {
            if (ModelState.IsValid)
            {
                T_USER beforeData = _db.T_USER.Single(e => e.ID == model.ID);

                if (uploadImage != null && uploadImage.ContentType.StartsWith("image/"))
                {
                        byte[] data = new Byte[uploadImage.ContentLength];
                        uploadImage.InputStream.Read(data, 0, uploadImage.ContentLength);
                        string mimeType = uploadImage.ContentType;
                        model.ICON = data;  // データ本体
                        model.MIMETYPE = mimeType;

                }
                else if(uploadImage == null)
                {
                    model.ICON = beforeData.ICON;
                    model.MIMETYPE = beforeData.MIMETYPE;
                }
                else
                {
                        // 画像ファイルでない場合はエラー
                        ViewData["msg"] = "画像以外はアップロードできません。";
                        return View("Edit", model);
                }
                //引数のmodelをそのまま突っ込むと新規追加扱いになる?
                //その結果PKの一意制約違反になるので登録できない。したがって変更した項目のみ代入する。
                beforeData.FULLNAME = model.FULLNAME;
                beforeData.ICON = model.ICON;
                beforeData.MIMETYPE = model.MIMETYPE;
                _db.Entry(beforeData).State = EntityState.Modified;
                _db.SaveChanges();

                //成功メッセージくらいは出す。
                TempData["resultText"] = "更新成功!";

                return RedirectToAction("Index");
            }
            return View("Edit", model);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            T_USER targetUser = _db.T_USER.Where(e => e.ID == id).FirstOrDefault();
            if (targetUser == null)
            {
                return HttpNotFound();
            }
            return View(targetUser);
        }

        /// <summary>
        /// 削除完了時
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExecuteDelete(string id)
        {
           T_USER user = _db.T_USER.Find(id);
           if (user != null)
           {
               _db.T_USER.Remove(user);
               _db.SaveChanges();
                //成功メッセージくらいは出す。
                TempData["resultText"] = "削除成功!";

            }
            else
            {
                TempData["resultText"] = "削除失敗!対象ユーザが見つかりませんでした。";
                TempData["isSuccess"] = false;

            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            base.Dispose(disposing);
        }

        /// <summary>
        /// 権限ドロップダウンリストの生成
        /// </summary>
        /// <param name="emptyOption">空オプション(選択してください)を含むかどうか</param>
        /// <returns></returns>
        private List<SelectListItem> CreateAdminDropDown(bool emptyOption = false)
        {
            List<SelectListItem> adminSelector = new List<SelectListItem>();
            if (emptyOption)
                adminSelector.Add(new SelectListItem
                {
                    Text = "選択してください",
                    Value = string.Empty,
                });

            adminSelector.Add(new SelectListItem
            {
                Text = "利用者",
                Value = "0",
            });
            adminSelector.Add(new SelectListItem
            {
                Text = "管理者",
                Value = "1",
            });

            return adminSelector;
        }
    }    
}
