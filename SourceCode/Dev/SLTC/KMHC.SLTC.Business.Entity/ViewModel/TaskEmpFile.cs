using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.Model;

namespace KMHC.SLTC.Business.Entity
{
    public class TaskEmpFile
    {
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
    }

    public class TaskEmpFileList
    {
        public List<TaskEmpFile> TaskEmpFiles { get; set; }
        public AssignTask OldTask { get; set; }
    }
}
