/*****************************************************************************
 * Creator:	Lei Chen
 * Create Date: 2016-03-23
 * Modifier:
 * Modify Date:
 * Description: 提供快速Code Mapping
 ******************************************************************************/
using KM.Common;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.Other
{
    public static class CodeHelper
    {
        private static IUnitOfWork unitOfWork = IOCContainer.Instance.Resolve<IUnitOfWork>();

        #region CodeValue

        public static List<CodeValue> GetCodeByItemType(string ItemType)
        {
            List<CodeValue> retCode = (from cd in AllCodeDtl.Where(x => x.ITEMTYPE == ItemType)
                                        join ct in AllCodeFile on cd.ITEMTYPE equals ct.ITEMTYPE
                                       select new CodeValue
                                        {
                                            ItemCode = cd.ITEMCODE,
                                            ItemType=cd.ITEMTYPE,
                                            ItemName = cd.ITEMNAME
                                        }).ToList();
            return retCode;
        }

        public static string GetItemName(string key, string ItemType)
        {
            List<CodeValue> entity = GetCodeByItemType(ItemType);
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    key = key.Trim();
                    if (entity.Where(x => x.ItemCode == key).Count() > 0)
                    {
                        return entity.Where(x => x.ItemCode == key).FirstOrDefault().ItemName;
                    }
                }
            }
            return "";
        }

        #endregion


        #region code table

        static IEnumerable<LTC_CODEDTL_REF> CodeDtl;
        static IEnumerable<LTC_CODEFILE_REF> CodeFile;

        private static void EnsureLTC_CODEDTL_REFORefresh()
        {
          CodeDtl= unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet;
        }

        private static void EnsureLTC_CODEFILE_REFORefresh()
        {
            CodeFile = unitOfWork.GetRepository<LTC_CODEFILE_REF>().dbSet;
        }


        public static IEnumerable<LTC_CODEDTL_REF> AllCodeDtl
        {
            get
            {
                if (CodeDtl == null)
                {
                    EnsureLTC_CODEDTL_REFORefresh();
                }
                return CodeDtl;
            }
        }

        public static IEnumerable<LTC_CODEFILE_REF> AllCodeFile
        {
            get
            {
                if (CodeFile == null)
                {
                    EnsureLTC_CODEFILE_REFORefresh();
                }
                return CodeFile;
            }
        } 

        #endregion


    }
}
