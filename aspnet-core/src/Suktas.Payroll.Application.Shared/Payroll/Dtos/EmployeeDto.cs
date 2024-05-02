using Suktas.Payroll.Municipality.Enum;

using System;
using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class EmployeeDto : EntityDto<Guid>
    {
        public EmployeeCategory Category { get; set; }

        public string ProvidentFund { get; set; }

        public string PanNo { get; set; }

        public string Name { get; set; }

        public string BankAccountNo { get; set; }

        public string DateOfJoinMiti { get; set; }

        public bool IsDearnessAllowance { get; set; }

        public bool IsPrincipal { get; set; }

        public bool IsGovernment { get; set; }

        public bool IsInternal { get; set; }

        public Guid EmployeeLevelId { get; set; }

        public Guid? SchoolInfoId { get; set; }

    }
}