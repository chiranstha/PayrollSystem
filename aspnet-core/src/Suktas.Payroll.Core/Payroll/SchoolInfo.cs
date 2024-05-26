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

        public virtual string Level { get; set; }
        public virtual int WardNo { get; set; }
        //File

        public virtual Guid? Image { get; set; } //File, (BinaryObjectId)

    }
}