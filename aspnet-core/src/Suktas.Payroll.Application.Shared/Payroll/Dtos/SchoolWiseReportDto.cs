using System;
using System.Collections.Generic;
using System.Text;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class SchoolWiseReportDto
    {
        public int Year { get; set; }
        public string Months { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
