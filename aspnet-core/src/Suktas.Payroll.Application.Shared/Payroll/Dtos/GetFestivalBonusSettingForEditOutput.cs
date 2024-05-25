using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using Suktas.Payroll.Municipality.Enum;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetFestivalBonusSettingForEditOutput : EntityDto<Guid?>
    {

        public Months MonthId { get; set; }
        public PercentOrAmount PercentOrAmount { get; set; }
        public decimal Value { get; set; }
        public string Remarks { get; set; }

    }
}