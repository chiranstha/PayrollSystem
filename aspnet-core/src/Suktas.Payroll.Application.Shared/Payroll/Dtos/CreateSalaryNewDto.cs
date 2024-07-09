using Suktas.Payroll.Municipality.Enum;
using System.Collections.Generic;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateSalaryNewDto
    {
        public List<Months> months { get; set; }
        public List<CreateEmployeeSalaryNewDto> data { get; set; }
        public int Year { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DueAmount { get; set; }
        public decimal ExtraAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string Remarks { get; set; }
    }
}
