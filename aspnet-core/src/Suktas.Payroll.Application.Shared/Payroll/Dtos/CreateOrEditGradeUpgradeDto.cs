using Suktas.Payroll.Municipality.Enum;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateOrEditGradeUpgradeDto : EntityDto<Guid?>
    {

        public string DateMiti { get; set; }

        public EmployeeGrade Grade { get; set; }

        public string Remarks { get; set; }

        public Guid? EmployeeId { get; set; }

    }
}