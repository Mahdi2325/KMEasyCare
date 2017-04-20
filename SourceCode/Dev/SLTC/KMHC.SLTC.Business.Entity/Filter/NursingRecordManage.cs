using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class tt 
    {

        public int ID { get; set; }



    }

    public class RecordFilter
    {
       // 姓名
        public string  Name { get; set; }

        public int RegNo { get; set; }

        public int ID { get; set; }

        public int FEENO { get; set; }

        //输出量
        public int outvalue { get; set; }
        //输入量
        public int intvalue { get; set; }

        //管路的三个变量       
        public Nullable<bool> NASOGASTRIC { get; set; }

         public Nullable<bool> CATHETER { get; set; }


         public Nullable<bool> TRACHEOSTOMY { get; set; }

         public int intcount { get; set; }
            

    }
}
    