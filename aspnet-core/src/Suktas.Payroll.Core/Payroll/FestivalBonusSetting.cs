using Suktas.Payroll.Municipality.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_FestivalBonusSettings")]
    public class FestivalBonusSetting : Entity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Months MonthId { get; set; }
        public virtual PercentOrAmount PercentOrAmount { get; set; }
        public virtual decimal Value { get; set; }

        public virtual string Remarks { get; set; }

    }
}