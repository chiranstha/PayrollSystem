using Abp.Domain.Entities;
using Suktas.Payroll.Municipality.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetMonthlyAllowanceForEdit : Entity<Guid>
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public EmployeeCategory EmployeeCategory { get; set; }

    }
}
