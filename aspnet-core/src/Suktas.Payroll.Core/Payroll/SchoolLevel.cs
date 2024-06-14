using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_SchoolLevel")]
    public class SchoolLevel : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual string AliasName { get; set; }

        [Required]
        public virtual string Name { get; set; }
    }
}
