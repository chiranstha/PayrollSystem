using Abp.Application.Services.Dto;
using Suktas.Payroll.Municipality.Enum;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetGradeUpgradeForViewDto : EntityDto<Guid>
    {
        public string DateMiti { get; set; }

        public EmployeeGrade Grade { get; set; }

        public string Remarks { get; set; }

        public Guid? EmployeeId { get; set; }

        public string EmployeeName { get; set; }

    }
}