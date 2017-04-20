using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class TableParam
    {
        public EnumFillType FillType { get; set; }
        public List<CellParam> Cells { get; set; }

        public TableParam()
        {
            this.Cells = new List<CellParam>();
        }
    }

    public class CellParam
    {
        public string FieldName { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
    }

    public enum EnumFillType
    {
        Horizontal,
        Vertical
    }
}





