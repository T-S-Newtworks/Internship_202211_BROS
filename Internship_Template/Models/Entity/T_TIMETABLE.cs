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
    
    public partial class T_TIMETABLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_TIMETABLE()
        {
            this.T_DATEANDTIME = new HashSet<T_DATEANDTIME>();
        }
    
        public string ID { get; set; }
        public string DIRECTION { get; set; }
        public string HITACHI_TIME { get; set; }
        public string HITACHITAGA_TIME { get; set; }
        public string TOKYO_TIME { get; set; }
        public string UENO_TIME { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_DATEANDTIME> T_DATEANDTIME { get; set; }
    }
}
