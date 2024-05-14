using System;
using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class PrincipalAllowanceSettingDto : EntityDto<Guid>
    {
        public decimal Amount { get; set; }

        public Guid EmployeeLevelId { get; set; }

    }
}