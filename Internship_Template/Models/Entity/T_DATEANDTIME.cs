//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Internship_Template.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class T_DATEANDTIME
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_DATEANDTIME()
        {
            this.T_RESERVE = new HashSet<T_RESERVE>();
        }
    
        public string ID { get; set; }
        public string TIMETABLEID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{YYYY/MM/DD}")]
        public Nullable<System.DateTime> YMD { get; set; }

        [NotMapped]
        public string YMDString { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_RESERVE> T_RESERVE { get; set; }
        public virtual T_TIMETABLE T_TIMETABLE { get; set; }
    }
}
