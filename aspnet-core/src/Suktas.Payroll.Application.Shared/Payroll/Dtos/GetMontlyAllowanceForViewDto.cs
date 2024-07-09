using Abp.Domain.Entities;
using Suktas.Payroll.Municipality.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetMontlyAllowanceForViewDto : Entity<Guid?>
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string EmployeeCategory { get; set; }
    }
}
