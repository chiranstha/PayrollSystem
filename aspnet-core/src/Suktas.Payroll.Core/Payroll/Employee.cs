﻿using Suktas.Payroll.Municipality.Enum;
using Suktas.Payroll.Payroll;
using Suktas.Payroll.Payroll;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_Employee")]
    public class Employee : Entity<Guid>, IMayHaveTenant
    {

        public virtual EmployeeCategory Category { get; set; }

        public virtual string ProvidentFund { get; set; }

        public virtual string PanNo { get; set; }

        public virtual string InsuranceNo { get; set; }

        public virtual string Name { get; set; }

        public virtual string BankName { get; set; }

        public virtual string BankAccountNo { get; set; }

        public virtual string PansionMiti { get; set; }

        public virtual string DateOfJoinMiti { get; set; }

        public virtual bool IsDearnessAllowance { get; set; }

        public virtual bool IsPrincipal { get; set; }

        public virtual bool IsGovernment { get; set; }

        public virtual bool IsInternal { get; set; }

        public virtual Guid EmployeeLevelId { get; set; }

        [ForeignKey("EmployeeLevelId")]
        public EmployeeLevel EmployeeLevelFk { get; set; }

        public virtual Guid? SchoolInfoId { get; set; }

        [ForeignKey("SchoolInfoId")]
        public SchoolInfo SchoolInfoFk { get; set; }
        public int? TenantId { get; set; }
    }
}