using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internship_Template.Models.Entity
{
    /// <summary>
    /// セッション文字列格納クラス
    /// </summary>
    public class M_SESSION
    {
        /// <summary>
        /// ログインユーザーのSessionKey。この識別子にはT_USERクラスの情報が入る。
        /// </summary>
        public const string SessionKey = "_LoginUser_";
    }
}