using System;
using System.Collections.Generic;
using System.Text;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateEmployeeSalaryNewMasterDto
    {
        public CreateEmployeeSalaryNewDto Total {  get; set; }
        public List<CreateEmployeeSalaryNewDto> Details { get; set; }
    }
}
