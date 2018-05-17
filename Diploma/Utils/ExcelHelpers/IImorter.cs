using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Utils.ExcelHelpers
{
    interface IImorter
    {
        List<string> Log { get; set; }
        bool ImportDataFromExcel(string path, int year);
        void SaveData();
        void WorkloadAssigmnent();
    }
}
