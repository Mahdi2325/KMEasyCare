using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
   public  class AssignTaskFilterByBobDu
    {
       public DateTime start { get; set; }
       public DateTime end { get; set; }
       public long feeno { get; set; }
    }
   public class AssignTaskJobFilter
   {
       public DateTime start { get; set; }
       public DateTime end { get; set; }
       public string Assignee { get; set; }
   }
}
