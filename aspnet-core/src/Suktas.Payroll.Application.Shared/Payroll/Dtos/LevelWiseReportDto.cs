using System;
using System.Collections.Generic;
using System.Text;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class LevelWiseReportDto
    {
        public string School { get; set; }
        public string EmpName { get; set; }
        public decimal Salary { get; set; }
        public string Months { get; set; }
    }
}
