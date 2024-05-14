using Suktas.Payroll.Municipality.Enum;

using System;
using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GradeUpgradeDto : EntityDto<Guid>
    {
        public string DateMiti { get; set; }

        public EmployeeGrade Grade { get; set; }

        public string Remarks { get; set; }

        public Guid? EmployeeId { get; set; }

    }
}