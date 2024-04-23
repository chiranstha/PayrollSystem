using System;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Municipality.Enum;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetInternalGradeSetupForViewDto : EntityDto<Guid>
    {
        public string Category { get; set; }

        public string Grade { get; set; }

        public bool IsPercent { get; set; }

        public decimal Value { get; set; }
    }
}