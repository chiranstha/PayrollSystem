using Suktas.Payroll.Municipality.Enum;

using System;
using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class InternalGradeSetupDto : EntityDto<Guid>
    {
        public EmployeeCategory Category { get; set; }

        public EmployeeGrade Grade { get; set; }

        public bool IsPercent { get; set; }

        public decimal Value { get; set; }

    }
}