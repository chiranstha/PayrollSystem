using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetPrincipalAllowanceSettingForEditOutput : EntityDto<Guid?>
    {
        public decimal Amount { get; set; }

        public Guid EmployeeLevelId { get; set; }

        public string EmployeeLevelName { get; set; }

    }
}