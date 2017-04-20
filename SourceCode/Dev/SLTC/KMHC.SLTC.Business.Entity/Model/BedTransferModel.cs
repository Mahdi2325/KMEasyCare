using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model {
	public class BedTransferModel {
		public long ID { get; set; }
		public long? FEENO { get; set; }
		public string EVENTTYPE { get; set; }
		public string DEPT_O { get; set; }
		public string FLOOR_O { get; set; }
		public string ROOM_O { get; set; }
		public string BEDNO_O { get; set; }
		public string DEPT_D { get; set; }
		public string FLOOR_D { get; set; }
		public string ROOMNO_D { get; set; }
		public string BEDNO_D { get; set; }
		public Nullable<System.DateTime> TRANDATE { get; set; }
		public string TRANDESC { get; set; }
		public Nullable<System.DateTime> UPDATEDATE { get; set; }
		public string UPDATEBY { get; set; }
		public string ORGID { get; set; }
	}

}
