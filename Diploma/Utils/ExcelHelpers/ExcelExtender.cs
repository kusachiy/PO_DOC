using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Utils.ExcelHelpers
{
    public static class ExcelExtender
    {
        public static string GetCellText(this Microsoft.Office.Interop.Excel.Worksheet worksheet, int row, int column)
        {
            return ((string)((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[row, column]).Text).Trim();
        }
    }
}
