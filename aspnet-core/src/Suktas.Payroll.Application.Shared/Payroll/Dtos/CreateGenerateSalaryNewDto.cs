using Suktas.Payroll.Municipality.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateGenerateSalaryNewDto
    {
        public List<Guid> SchoolIds {  get; set; }
        public int Year { get; set; }
        public List<Months> Months { get; set; }
    }
}
