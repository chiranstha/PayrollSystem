using Abp.Domain.Entities;
using Suktas.Payroll.Municipality.Enum;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateOrEditMontlyAllowanceDto : Entity<Guid>
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public EmployeeCategory EmployeeCategory { get; set; }
    }
}
