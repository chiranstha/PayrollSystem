using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_SchoolInfo")]
    public class SchoolInfo : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual string Address { get; set; }

        public virtual string PhoneNo { get; set; }

        public virtual string Email { get; set; }

        public virtual string Description { get; set; }

        public virtual Guid SchooolLevelId { get; set; }

        [ForeignKey("SchooolLevelId")]
        public virtual SchoolLevel SchoolLevelFk { get; set; }
        public virtual int WardNo { get; set; }
        public virtual Guid? Image { get; set; }

    }
}