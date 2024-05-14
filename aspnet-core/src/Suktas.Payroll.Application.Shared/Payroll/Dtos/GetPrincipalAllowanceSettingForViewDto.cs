using Abp.Application.Services.Dto;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetPrincipalAllowanceSettingForViewDto : EntityDto<Guid>
    {
        public decimal Amount { get; set; }

        public Guid EmployeeLevelId { get; set; }

        public string EmployeeLevelName { get; set; }

    }
}