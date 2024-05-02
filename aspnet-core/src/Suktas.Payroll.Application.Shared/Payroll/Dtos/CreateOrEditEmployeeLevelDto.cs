using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using Suktas.Payroll.Municipality.Enum;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateOrEditEmployeeLevelDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        public EmployeeGrade Grade { get; set; }
    }
}