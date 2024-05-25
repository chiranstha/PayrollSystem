using Abp.Application.Services.Dto;
using Suktas.Payroll.Municipality.Enum;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetFestivalBonusSettingForViewDto : EntityDto<Guid>
    {
        public Months MonthId { get; set; }
        public virtual PercentOrAmount PercentOrAmount { get; set; }
        public virtual decimal Value { get; set; }
        public string Remarks { get; set; }

    }
}