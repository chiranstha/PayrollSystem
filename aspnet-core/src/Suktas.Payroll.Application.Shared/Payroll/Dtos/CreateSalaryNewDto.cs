using Suktas.Payroll.Municipality.Enum;
using System;
using System.Collections.Generic;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateSalaryNewDto
    {
        public List<Guid> schoolIds { get; set; }
        public List<Months> months { get; set; }
        public List<CreateEmployeeSalaryNewDto> data { get; set; }
        public int Year { get; set; }
    }
}
