using Abp.Application.Services.Dto;
using Suktas.Payroll.Municipality.Enum;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetFestivalBonusSettingForViewDto : EntityDto<Guid>
    {
        public Months MonthId { get; set; }

        public string Remarks { get; set; }

    }
}