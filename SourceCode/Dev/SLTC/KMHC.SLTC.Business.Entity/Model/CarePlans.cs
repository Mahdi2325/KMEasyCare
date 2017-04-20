using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class CarePlans
    {
    }

    public class CodeValueComparer : IEqualityComparer<CodeValue>
    {

        public bool Equals(CodeValue p1, CodeValue p2)
        {

            if (p1 == null)

                return p2 == null;

            return p1.ItemCode.Trim() == p2.ItemCode.Trim() && p1.ItemName.Trim() == p2.ItemName.Trim();

        }



        public int GetHashCode(CodeValue p)
        {

            if (p == null)

                return 0;

            return p.ItemCode.GetHashCode();

        }

    }
}
