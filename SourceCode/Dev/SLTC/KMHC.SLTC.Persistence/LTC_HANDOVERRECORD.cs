//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KMHC.SLTC.Persistence
{
    using System;
    using System.Collections.Generic;
    
    public partial class LTC_HANDOVERRECORD
    {
        public LTC_HANDOVERRECORD()
        {
            this.LTC_HANDOVERDTL = new HashSet<LTC_HANDOVERDTL>();
        }
    
        public long ID { get; set; }
        public Nullable<System.DateTime> HANDOVERDATE { get; set; }
        public Nullable<int> NEWCOMER { get; set; }
        public Nullable<int> TRANSFERSOCIETY { get; set; }
        public Nullable<int> TRANSFERMINITER { get; set; }
        public Nullable<int> TRANSFERDISABLED { get; set; }
        public Nullable<int> OUTOVERALL { get; set; }
        public Nullable<int> OUTRETURN { get; set; }
        public Nullable<int> OUTSTILL { get; set; }
        public Nullable<int> ACTUALPOPULATION { get; set; }
        public Nullable<int> INNAISOCIETY { get; set; }
        public Nullable<int> INNAIMINITER { get; set; }
        public Nullable<int> INNAIDISABLED { get; set; }
        public Nullable<int> INNAIOVERALL { get; set; }
    
        public virtual ICollection<LTC_HANDOVERDTL> LTC_HANDOVERDTL { get; set; }
    }
}