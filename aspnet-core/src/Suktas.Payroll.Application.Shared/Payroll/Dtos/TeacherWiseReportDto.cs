using System;
using System.Collections.Generic;
using System.Text;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class TeacherWiseReportDto
    {
        public decimal BasicSalary { get; set; }
        public int Grade { get; set; }
        public decimal GradeRate { get; set; }
        public decimal GradeAmount { get; set; }
        public decimal TechnicalGradeAmount { get; set; }
        public decimal TotalGradeAmount { get; set; }
        public decimal Total { get; set; }
        public decimal EPFAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal InflationAllowance { get; set; }
        public decimal PrincipalAllowance { get; set; }
        public decimal TotalSalaryAmount { get; set; }
        public int Month { get; set; }
        public string MonthNames { get; set; }
        public decimal TotalForAllMonths { get; set; }
        public decimal FestivalAllowance { get; set; } = 0;
        public decimal TotalWithAllowanceForAllMonths { get; set; }
        public decimal InternalAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
    }
}
