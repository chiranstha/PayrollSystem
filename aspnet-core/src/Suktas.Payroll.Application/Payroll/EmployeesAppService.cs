using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Suktas.Payroll.Authorization;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Municipality.Enum;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Payroll.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    [AbpAuthorize(AppPermissions.Pages_Employees)]
    public class EmployeesAppService : PayrollAppServiceBase, IEmployeesAppService
    {
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IEmployeesExcelExporter _employeesExcelExporter;
        private readonly IRepository<EmployeeLevel, Guid> _lookupEmployeeLevelRepository;
        private readonly IRepository<SchoolInfo, Guid> _lookupSchoolInfoRepository;
        private readonly IRepository<GradeUpgrade, Guid> _gradeUpgradeRepository;
        public EmployeesAppService(IRepository<Employee, Guid> employeeRepository,
            IRepository<GradeUpgrade, Guid> gradeUpgradeRepository, IEmployeesExcelExporter employeesExcelExporter, IRepository<EmployeeLevel, Guid> lookupEmployeeLevelRepository, IRepository<SchoolInfo, Guid> lookupSchoolInfoRepository)
        {
            _employeeRepository = employeeRepository;
            _employeesExcelExporter = employeesExcelExporter;
            _lookupEmployeeLevelRepository = lookupEmployeeLevelRepository;
            _lookupSchoolInfoRepository = lookupSchoolInfoRepository;
            _gradeUpgradeRepository = gradeUpgradeRepository;
        }

        public virtual async Task<PagedResultDto<GetEmployeeForViewDto>> GetAll(GetAllEmployeesInput input)
        {
            var categoryFilter = input.CategoryFilter.HasValue
                        ? (EmployeeCategory)input.CategoryFilter
                        : default;

            var filteredEmployees = _employeeRepository.GetAll()
                        .Include(e => e.EmployeeLevelFk)
                        .Include(e => e.SchoolInfoFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.ProvidentFund.Contains(input.Filter) || e.PanNo.Contains(input.Filter) || e.InsuranceNo.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.BankName.Contains(input.Filter) || e.BankAccountNo.Contains(input.Filter) || e.PansionMiti.Contains(input.Filter) || e.DateOfJoinMiti.Contains(input.Filter))
                        .WhereIf(input.CategoryFilter.HasValue && input.CategoryFilter > -1, e => e.Category == categoryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PanNoFilter), e => e.PanNo.Contains(input.PanNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InsuranceNoFilter), e => e.InsuranceNo.Contains(input.InsuranceNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeLevelNameFilter), e => e.EmployeeLevelFk != null && e.EmployeeLevelFk.Name == input.EmployeeLevelNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SchoolInfoNameFilter), e => e.SchoolInfoFk != null && e.SchoolInfoFk.Name == input.SchoolInfoNameFilter);

            var pagedAndFilteredEmployees = filteredEmployees
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employees = from o in pagedAndFilteredEmployees
                            join o1 in _lookupEmployeeLevelRepository.GetAll() on o.EmployeeLevelId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookupSchoolInfoRepository.GetAll() on o.SchoolInfoId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new
                            {

                                o.Category,
                                o.ProvidentFund,
                                o.PanNo,
                                o.Name,
                                o.BankAccountNo,
                                o.DateOfJoinMiti,
                                o.IsDearnessAllowance,
                                o.IsPrincipal,
                                o.IsGovernment,
                                o.IsInternal,
                                o.Id,
                                EmployeeLevelName = s1 == null || s1.Name == null ? "" : s1.Name,
                                SchoolInfoName = s2 == null || s2.Name == null ? "" : s2.Name
                            };

            var totalCount = await filteredEmployees.CountAsync();

            var dbList = await employees.ToListAsync();
            var results = dbList.Select(o => new GetEmployeeForViewDto
            {
                Category = o.Category.ToString(),
                ProvidentFund = o.ProvidentFund,
                PanNo = o.PanNo,
                Name = o.Name,
                BankAccountNo = o.BankAccountNo,
                DateOfJoinMiti = o.DateOfJoinMiti,
                IsDearnessAllowance = o.IsDearnessAllowance,
                IsPrincipal = o.IsPrincipal,
                IsGovernment = o.IsGovernment,
                IsInternal = o.IsInternal,
                Id = o.Id,

                EmployeeLevelName = o.EmployeeLevelName,
                SchoolInfoName = o.SchoolInfoName
            })
                .ToList();

            return new PagedResultDto<GetEmployeeForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetEmployeeForViewDto> GetEmployeeForView(Guid id)
        {
            var o = await _employeeRepository.GetAll().Include(e => e.EmployeeLevelFk)
                .Include(e => e.SchoolInfoFk).FirstOrDefaultAsync(e => e.Id == id);


            var output = new GetEmployeeForViewDto
            {
                Category = o.Category.ToString(),
                ProvidentFund = o.ProvidentFund,
                PanNo = o.PanNo,
                Name = o.Name,
                BankAccountNo = o.BankAccountNo,
                DateOfJoinMiti = o.DateOfJoinMiti,
                IsDearnessAllowance = o.IsDearnessAllowance,
                IsPrincipal = o.IsPrincipal,
                IsGovernment = o.IsGovernment,
                IsInternal = o.IsInternal,
                Id = o.Id,

                EmployeeLevelName = o.EmployeeLevelFk.Name,
                SchoolInfoName = o.SchoolInfoFk.Name
            };



            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Employees_Edit)]
        public virtual async Task<CreateOrEditEmployeeDto> GetEmployeeForEdit(EntityDto<Guid> input)
        {
            var employee = await _employeeRepository.FirstOrDefaultAsync(input.Id);

            var output = new CreateOrEditEmployeeDto
            {
                Id = employee.Id,
                Category = employee.Category,
                ProvidentFund = employee.ProvidentFund,
                PanNo = employee.PanNo,
                InsuranceNo = employee.InsuranceNo,
                Name = employee.Name,
                BankName = employee.BankName,
                BankAccountNo = employee.BankAccountNo,
                PansionMiti = employee.PansionMiti,
                DateOfJoinMiti = employee.DateOfJoinMiti,
                IsDearnessAllowance = employee.IsDearnessAllowance,
                IsPrincipal = employee.IsPrincipal,
                IsGovernment = employee.IsGovernment,
                IsInternal = employee.IsInternal,
                EmployeeLevelId = employee.EmployeeLevelId,
                SchoolInfoId = employee.SchoolInfoId
            };
            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditEmployeeDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Employees_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeDto input)
        {
            var employee = new Employee
            {
                Id = default,
                Category = input.Category,
                ProvidentFund = input.ProvidentFund,
                PanNo = input.PanNo,
                InsuranceNo = input.InsuranceNo,
                Name = input.Name,
                BankName = input.BankName,
                BankAccountNo = input.BankAccountNo,
                PansionMiti = input.PansionMiti,
                DateOfJoinMiti = input.DateOfJoinMiti,
                IsDearnessAllowance = input.IsDearnessAllowance,
                IsPrincipal = input.IsPrincipal,
                IsGovernment = input.IsGovernment,
                IsInternal = input.IsInternal,
                EmployeeLevelId = input.EmployeeLevelId,
                SchoolInfoId = input.SchoolInfoId,
                IsTechnical = input.IsTechnical,
                TenantId = AbpSession.GetTenantId()
            };
            var empId = await _employeeRepository.InsertAndGetIdAsync(employee);

            var empGrade = new GradeUpgrade
            {
                DateMiti = DateTime.Today.ToString(),
                Grade = input.Grade,
                Remarks = "",
                IsActive = true,
                EmployeeId = empId,
                TenantId = AbpSession.GetTenantId()
            };
            await _gradeUpgradeRepository.InsertAsync(empGrade);
        }

        [AbpAuthorize(AppPermissions.Pages_Employees_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeDto input)
        {
            var employee = await _employeeRepository.FirstOrDefaultAsync(e => e.Id == input.Id) ?? throw new UserFriendlyException("Data not found");
            employee.Category = input.Category;
            employee.ProvidentFund = input.ProvidentFund;
            employee.PanNo = input.PanNo;
            employee.InsuranceNo = input.InsuranceNo;
            employee.Name = input.Name;
            employee.BankName = input.BankName;
            employee.BankAccountNo = input.BankAccountNo;
            employee.PansionMiti = input.PansionMiti;
            employee.DateOfJoinMiti = input.DateOfJoinMiti;
            employee.IsDearnessAllowance = input.IsDearnessAllowance;
            employee.IsPrincipal = input.IsPrincipal;
            employee.IsGovernment = input.IsGovernment;
            employee.IsInternal = input.IsInternal;
            employee.EmployeeLevelId = input.EmployeeLevelId;
            employee.SchoolInfoId = input.SchoolInfoId;
            employee.IsTechnical = input.IsTechnical;
            await _employeeRepository.UpdateAsync(employee);
        }

        [AbpAuthorize(AppPermissions.Pages_Employees_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            var employee = await _employeeRepository.FirstOrDefaultAsync(e => e.Id == input.Id) ?? throw new UserFriendlyException("Data not found");
            await _employeeRepository.DeleteAsync(employee);
        }

        public virtual async Task<FileDto> GetEmployeesToExcel(GetAllEmployeesForExcelInput input)
        {
            var categoryFilter = input.CategoryFilter.HasValue
                        ? (EmployeeCategory)input.CategoryFilter
                        : default;

            var filteredEmployees = _employeeRepository.GetAll()
                        .Include(e => e.EmployeeLevelFk)
                        .Include(e => e.SchoolInfoFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.ProvidentFund.Contains(input.Filter) || e.PanNo.Contains(input.Filter) || e.InsuranceNo.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.BankName.Contains(input.Filter) || e.BankAccountNo.Contains(input.Filter) || e.PansionMiti.Contains(input.Filter) || e.DateOfJoinMiti.Contains(input.Filter))
                        .WhereIf(input.CategoryFilter.HasValue && input.CategoryFilter > -1, e => e.Category == categoryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PanNoFilter), e => e.PanNo.Contains(input.PanNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InsuranceNoFilter), e => e.InsuranceNo.Contains(input.InsuranceNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeLevelNameFilter), e => e.EmployeeLevelFk != null && e.EmployeeLevelFk.Name == input.EmployeeLevelNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SchoolInfoNameFilter), e => e.SchoolInfoFk != null && e.SchoolInfoFk.Name == input.SchoolInfoNameFilter);

            var query = (from o in filteredEmployees
                         join o1 in _lookupEmployeeLevelRepository.GetAll() on o.EmployeeLevelId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookupSchoolInfoRepository.GetAll() on o.SchoolInfoId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetEmployeeForViewDto
                         {

                             Category = o.Category.ToString(),
                             ProvidentFund = o.ProvidentFund,
                             PanNo = o.PanNo,
                             Name = o.Name,
                             BankAccountNo = o.BankAccountNo,
                             DateOfJoinMiti = o.DateOfJoinMiti,
                             IsDearnessAllowance = o.IsDearnessAllowance,
                             IsPrincipal = o.IsPrincipal,
                             IsGovernment = o.IsGovernment,
                             IsInternal = o.IsInternal,
                             Id = o.Id,

                             EmployeeLevelName = s1 == null || s1.Name == null ? "" : s1.Name,
                             SchoolInfoName = s2 == null || s2.Name == null ? "" : s2.Name
                         });

            var employeeListDtos = await query.ToListAsync();

            return _employeesExcelExporter.ExportToFile(employeeListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Employees)]
        public async Task<List<EmployeeEmployeeLevelLookupTableDto>> GetAllEmployeeLevelForTableDropdown()
        {
            return await _lookupEmployeeLevelRepository.GetAll()
                .Select(employeeLevel => new EmployeeEmployeeLevelLookupTableDto
                {
                    Id = employeeLevel.Id.ToString(),
                    DisplayName = employeeLevel == null || employeeLevel.Name == null ? "" : employeeLevel.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Employees)]
        public async Task<List<EmployeeSchoolInfoLookupTableDto>> GetAllSchoolInfoForTableDropdown()
        {
            return await _lookupSchoolInfoRepository.GetAll()
                .Select(schoolInfo => new EmployeeSchoolInfoLookupTableDto
                {
                    Id = schoolInfo.Id.ToString(),
                    DisplayName = schoolInfo == null || schoolInfo.Name == null ? "" : schoolInfo.Name.ToString()
                }).ToListAsync();
        }

    }
}