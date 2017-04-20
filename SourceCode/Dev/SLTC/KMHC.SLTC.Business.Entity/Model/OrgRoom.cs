/*
 * 描述:OrgRoom
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 3/24/2016 11:35:21 AM    张正泉            15986707042@163.com    创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class OrgRoom
    {
        public string RoomNo { get; set; }
        public string FloorId { get; set; }
        public string FloorName { get; set; }
        public string RoomName { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }

        public List<BedBasic> Bedes { get; set; }

        public int TotalBedNumber
        {
            get { return Bedes == null ? 0 : Bedes.Count; }
        }

        public int UsedBedNumber
        {
            get { return Bedes == null ? 0 : Bedes.Count(m => m.BedStatus == BedStatus.Used.ToString()); }
        }
        public int EmptyBedNumber
        {
            get { return TotalBedNumber - UsedBedNumber; }
        }

        #region Add By Duke
        //区域
        public string Area { get; set; }
        //房间类型
        public string RoomType { get; set; }
        #endregion
    }
}
