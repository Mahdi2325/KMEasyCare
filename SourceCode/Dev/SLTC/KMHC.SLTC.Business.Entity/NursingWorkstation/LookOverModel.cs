using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NursingWorkstation
{
    public class LookOverModel
    {
        public long ID { get; set; }
        public System.DateTime? LookOverTime { get; set; }
        public string ItemCode { get; set; }
        public string FloorId { get; set; }
        public string FloorName { get; set; }
        public string LookOverPhotos { get; set; }
        public string Content { get; set; }
        public string RecordBy { get; set; }
        public string RecordName { get; set; }
        public string[] PhotoList { get; set; }
    }
}
