using Suktas.Payroll.Municipality.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Suktas.Payroll.Payroll
{
    [Table("FestivalBonusSettings")]
    public class FestivalBonusSetting : Entity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Months MonthId { get; set; }

        public virtual string Remarks { get; set; }

    }
}