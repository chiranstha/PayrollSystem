using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using Suktas.Payroll.Municipality.Enum;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetGradeUpgradeForEditOutput : EntityDto<Guid?>
    {

        public string DateMiti { get; set; }

        public EmployeeGrade Grade { get; set; }

        public string Remarks { get; set; }

        public Guid? EmployeeId { get; set; }

        public string EmployeeName { get; set; }

    }
}