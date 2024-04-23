using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using Suktas.Payroll.Municipality.Enum;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetInternalGradeSetupForEditOutput : EntityDto<Guid>
    {
        public EmployeeCategory Category { get; set; }

        public EmployeeGrade Grade { get; set; }

        public bool IsPercent { get; set; }

        public decimal Value { get; set; }
    }
}