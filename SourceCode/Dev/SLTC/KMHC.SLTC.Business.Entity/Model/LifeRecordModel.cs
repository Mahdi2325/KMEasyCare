using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class LifeRecordModel
    {
        public long Id;
        public long? FeeNo;
        public string Name;
        public int? RegNo;
        public DateTime? RecordDate;
        public string AmActivity;
        public string PmActivity;
        public string Comments;
        public decimal? BodyTemp;
        public string RecordBy;
        public string RecordByName;
        public DateTime? CreateDate;
        public string CreateBy;
        public string OrgId;
        public string Floor;
        public string RoomNo;
        public string BedNo;
        public string  ResidentsNo { get; set; }
    }
    public class LifeRecordListModel
    {
		public int? Id { get; set; }
        public int? RegNo;
        public long FeeNo;
        public string BedNo;
        public string Floor;
		public string FloorName { get; set; }
		public string RoomNo { get; set; }
		public string RoomName { get; set; }
        public string Name;
        public DateTime? RecordDate ;
        public string RecordBy;
        public decimal? BodyTemp;
        public string RecordByName;
        public string AmActivity;
        public string PmActivity;
        public string Comments;
        public long? RecordId;
        public bool CheckType;
    }
}
