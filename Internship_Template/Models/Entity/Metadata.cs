using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Internship_Template.Models.Entity
{

    public class T_USERMetadata
    {
        [StringLength(10)]
        [Required(ErrorMessage = "{0}は必須です。")]
        [Display(Name ="ユーザーID")]
        public string ID;

        [StringLength(50)]
        [Display(Name = "フルネーム")]
        public string FULLNAME;

        [Display(Name = "アイコン")]
        public byte[] ICON;

        [StringLength(100)]
        public string MIMETYPE;

        [Required(ErrorMessage = "{0}は必須です。")]
        [Display(Name = "管理者権限")]
        [StringLength(1)]
        public string ADMINFLG;

    }

    public class T_LOGINMetadata
    {
        [StringLength(10)]
        [Required(ErrorMessage = "{0}は必須です。")]
        public string ID { get; set; }

        [Required(ErrorMessage = "{0}は必須です。")]
        [Display(Name = "PASSWORD")]
        [StringLength(20)]
        public string PASSWORD { get; set; }

    }

}