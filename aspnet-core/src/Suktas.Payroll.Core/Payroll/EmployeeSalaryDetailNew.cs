using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_EmployeeSalaryDetailNew")]
    public class EmployeeSalaryDetailNew : Entity<Guid>, IMayHaveTenant
    {
        public int SN { get; set; }
        public int WardNo { get; set; }
        public string SchoolLevel { get; set; }
        public string SchoolName { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeLevel { get; set; }
        public string EmployeeName { get; set; }
        public decimal BasicSalary { get; set; }
        public int Grade { get; set; }
        public decimal GradeRate { get; set; }
        public decimal GradeAmount { get; set; }
        public decimal TechnicalGradeAmount { get; set; }
        public decimal TotalGradeAmount { get; set; }
        public decimal Total { get; set; }
        public decimal EPFAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal InflationAllowance { get; set; }
        public decimal PrincipalAllowance { get; set; }
        public decimal TotalSalaryAmount { get; set; }
        public int Month { get; set; }
        public string MonthNames { get; set; }
        public decimal TotalForAllMonths { get; set; }
        public decimal FestivalAllowance { get; set; } = 0;
        public decimal TotalWithAllowanceForAllMonths { get; set; }
        public decimal InternalAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public string Remarks { get; set; }
        public bool IsPaid { get; set; }
        public int? TenantId { get ; set; }
        public virtual Guid SchoolInfoId { get; set; }

        [ForeignKey("SchoolInfoId")]
        public SchoolInfo SchoolInfoFk { get; set; }

        public virtual Guid EmployeeSalaryMasterNewId { get; set; }

        [ForeignKey("EmployeeSalaryMasterNewId")]
        public EmployeeSalaryMasterNew EmployeeSalaryMasterNewFk { get; set; }
    }
}
