using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suktas.Payroll.Payroll
{

    [Table("tbl_EmployeeSalaryMasterSchoolNew")]
    public class EmployeeSalaryMasterSchoolNew : Entity<Guid>, IMayHaveTenant
    {
        public Guid SchoolInfoId { get; set; }
        [ForeignKey("SchoolInfoId")]
        public SchoolInfo SchoolInfoFk { get; set; }
        public Guid EmployeeSalaryMasterNewId { get; set; }
        [ForeignKey("EmployeeSalaryMasterNewId")]
        public EmployeeSalaryMasterNew EmployeeSalaryMasterNewFk { get; set; }
        public int? TenantId { get; set; }
    }
}
