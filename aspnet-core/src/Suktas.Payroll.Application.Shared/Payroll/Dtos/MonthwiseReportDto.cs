using System;
using System.Collections.Generic;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class MonthwiseReportDto
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public List<SchoolWithAmount> SchoolWithAmounts { get; set; }
    }
    public class SchoolWithAmount
    {
        public string SchoolNames { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
