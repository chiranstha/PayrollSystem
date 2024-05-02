using Suktas.Payroll.Municipality.Enum;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Suktas.Payroll.Payroll.Exporting;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Authorization;
using Abp.Authorization;
using Abp.UI;
using Microsoft.EntityFrameworkCore;

namespace Suktas.Payroll.Payroll
{
    [AbpAuthorize(AppPermissions.Pages_EmployeeSalary)]
    public class EmployeeSalaryAppService : PayrollAppServiceBase, IEmployeeSalaryAppService
    {
        private readonly IRepository<EmployeeSalary, Guid> _employeeSalaryRepository;
        private readonly IEmployeeSalaryExcelExporter _employeeSalaryExcelExporter;
        private readonly IRepository<SchoolInfo, Guid> _lookupSchoolInfoRepository;
        private readonly IRepository<Employee, Guid> _lookupEmployeeRepository;
        private readonly IRepository<EmployeeLevel, Guid> _lookupEmployeeLevelRepository;

        public EmployeeSalaryAppService(IRepository<EmployeeSalary, Guid> employeeSalaryRepository,
            IEmployeeSalaryExcelExporter employeeSalaryExcelExporter,
            IRepository<SchoolInfo, Guid> lookupSchoolInfoRepository,
            IRepository<Employee, Guid> lookupEmployeeRepository,
            IRepository<EmployeeLevel, Guid> lookupEmployeeLevelRepository)
        {
            _employeeSalaryRepository = employeeSalaryRepository;
            _employeeSalaryExcelExporter = employeeSalaryExcelExporter;
            _lookupSchoolInfoRepository = lookupSchoolInfoRepository;
            _lookupEmployeeRepository = lookupEmployeeRepository;
            _lookupEmployeeLevelRepository = lookupEmployeeLevelRepository;
        }

        public virtual async Task<PagedResultDto<GetEmployeeSalaryForViewDto>> GetAll(GetAllEmployeeSalaryInput input)
        {
            var monthFilter = input.MonthFilter.HasValue
                ? (Months)input.MonthFilter
                : default;

            var filteredEmployeeSalary = _employeeSalaryRepository.GetAll()
                .Include(e => e.SchoolInfoFk)
                .Include(e => e.EmployeeFk)
                .Include(e => e.EmployeeLevelFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.DateMiti.Contains(input.Filter))
                .WhereIf(input.MonthFilter.HasValue && input.MonthFilter > -1, e => e.Month == monthFilter)
                .WhereIf(input.MinTechnicalAmountFilter != null,
                    e => e.TechnicalAmount >= input.MinTechnicalAmountFilter)
                .WhereIf(input.MaxTechnicalAmountFilter != null,
                    e => e.TechnicalAmount <= input.MaxTechnicalAmountFilter)
                .WhereIf(input.MinTotalGradeAmountFilter != null,
                    e => e.TotalGradeAmount >= input.MinTotalGradeAmountFilter)
                .WhereIf(input.MaxTotalGradeAmountFilter != null,
                    e => e.TotalGradeAmount <= input.MaxTotalGradeAmountFilter)
                .WhereIf(input.MinInsuranceAmountFilter != null,
                    e => e.InsuranceAmount >= input.MinInsuranceAmountFilter)
                .WhereIf(input.MaxInsuranceAmountFilter != null,
                    e => e.InsuranceAmount <= input.MaxInsuranceAmountFilter)
                .WhereIf(input.MinTotalSalaryFilter != null, e => e.TotalSalary >= input.MinTotalSalaryFilter)
                .WhereIf(input.MaxTotalSalaryFilter != null, e => e.TotalSalary <= input.MaxTotalSalaryFilter)
                .WhereIf(input.MinDearnessAllowanceFilter != null,
                    e => e.DearnessAllowance >= input.MinDearnessAllowanceFilter)
                .WhereIf(input.MaxDearnessAllowanceFilter != null,
                    e => e.DearnessAllowance <= input.MaxDearnessAllowanceFilter)
                .WhereIf(input.MinTotalWithAllowanceFilter != null,
                    e => e.TotalWithAllowance >= input.MinTotalWithAllowanceFilter)
                .WhereIf(input.MaxTotalWithAllowanceFilter != null,
                    e => e.TotalWithAllowance <= input.MaxTotalWithAllowanceFilter)
                .WhereIf(input.MinGovernmentAmountFilter != null,
                    e => e.GovernmentAmount >= input.MinGovernmentAmountFilter)
                .WhereIf(input.MaxGovernmentAmountFilter != null,
                    e => e.GovernmentAmount <= input.MaxGovernmentAmountFilter)
                .WhereIf(input.MinInternalAmountFilter != null, e => e.InternalAmount >= input.MinInternalAmountFilter)
                .WhereIf(input.MaxInternalAmountFilter != null, e => e.InternalAmount <= input.MaxInternalAmountFilter)
                .WhereIf(input.MinPaidSalaryAmountFilter != null,
                    e => e.PaidSalaryAmount >= input.MinPaidSalaryAmountFilter)
                .WhereIf(input.MaxPaidSalaryAmountFilter != null,
                    e => e.PaidSalaryAmount <= input.MaxPaidSalaryAmountFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SchoolInfoNameFilter),
                    e => e.SchoolInfoFk != null && e.SchoolInfoFk.Name == input.SchoolInfoNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter),
                    e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeLevelNameFilter),
                    e => e.EmployeeLevelFk != null && e.EmployeeLevelFk.Name == input.EmployeeLevelNameFilter);

            var pagedAndFilteredEmployeeSalary = filteredEmployeeSalary
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employeeSalary = from o in pagedAndFilteredEmployeeSalary
                join o1 in _lookupSchoolInfoRepository.GetAll() on o.SchoolInfoId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookupEmployeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _lookupEmployeeLevelRepository.GetAll() on o.EmployeeLevelId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                select new
                {
                    o.Month,
                    o.DateMiti,
                    o.BasicSalary,
                    o.GradeAmount,
                    o.TechnicalAmount,
                    o.TotalGradeAmount,
                    o.TotalBasicSalary,
                    o.InsuranceAmount,
                    o.TotalSalary,
                    o.DearnessAllowance,
                    o.PrincipalAllowance,
                    o.TotalWithAllowance,
                    o.TotalMonth,
                    o.TotalSalaryAmount,
                    o.FestiableAllowance,
                    o.GovernmentAmount,
                    o.InternalAmount,
                    o.PaidSalaryAmount,
                    o.Id,
                    SchoolInfoName = s1 == null || s1.Name == null ? "" : s1.Name,
                    EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name,
                    EmployeeLevelName = s3 == null || s3.Name == null ? "" : s3.Name
                };

            var totalCount = await filteredEmployeeSalary.CountAsync();

            var dbList = await employeeSalary.ToListAsync();
            var results = new List<GetEmployeeSalaryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetEmployeeSalaryForViewDto
                {
                    Month = o.Month.ToString(),
                    DateMiti = o.DateMiti,
                    BasicSalary = o.BasicSalary,
                    GradeAmount = o.GradeAmount,
                    TechnicalAmount = o.TechnicalAmount,
                    TotalGradeAmount = o.TotalGradeAmount,
                    TotalBasicSalary = o.TotalBasicSalary,
                    InsuranceAmount = o.InsuranceAmount,
                    TotalSalary = o.TotalSalary,
                    DearnessAllowance = o.DearnessAllowance,
                    PrincipalAllowance = o.PrincipalAllowance,
                    TotalWithAllowance = o.TotalWithAllowance,
                    TotalMonth = o.TotalMonth,
                    TotalSalaryAmount = o.TotalSalaryAmount,
                    FestiableAllowance = o.FestiableAllowance,
                    GovernmentAmount = o.GovernmentAmount,
                    InternalAmount = o.InternalAmount,
                    PaidSalaryAmount = o.PaidSalaryAmount,
                    Id = o.Id,

                    SchoolInfoName = o.SchoolInfoName,
                    EmployeeName = o.EmployeeName,
                    EmployeeLevelName = o.EmployeeLevelName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetEmployeeSalaryForViewDto>(
                totalCount,
                results
            );
        }

        public virtual async Task<GetEmployeeSalaryForViewDto> GetEmployeeSalaryForView(Guid id)
        {
            var data = await _employeeSalaryRepository.GetAll().Include(e => e.EmployeeLevelFk)
                .Include(e => e.EmployeeFk)
                .Include(e => e.SchoolInfoFk)
                .FirstOrDefaultAsync(e => e.Id == id);

            var output = new GetEmployeeSalaryForViewDto
            {
                Month = data.Month.ToString(),
                DateMiti = data.DateMiti,
                BasicSalary = data.BasicSalary,
                GradeAmount = data.GradeAmount,
                TechnicalAmount = data.TechnicalAmount,
                TotalGradeAmount = data.TotalGradeAmount,
                TotalBasicSalary = data.TotalBasicSalary,
                InsuranceAmount = data.InsuranceAmount,
                TotalSalary = data.TotalSalary,
                DearnessAllowance = data.DearnessAllowance,
                PrincipalAllowance = data.PrincipalAllowance,
                TotalWithAllowance = data.TotalWithAllowance,
                TotalMonth = data.TotalMonth,
                TotalSalaryAmount = data.TotalSalaryAmount,
                FestiableAllowance = data.FestiableAllowance,
                GovernmentAmount = data.GovernmentAmount,
                InternalAmount = data.InternalAmount,
                PaidSalaryAmount = data.PaidSalaryAmount,
                Id = data.Id,

                SchoolInfoName = data.SchoolInfoFk.Name,
                EmployeeName = data.EmployeeFk.Name,
                EmployeeLevelName = data.EmployeeLevelFk.Name
            };


            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeSalary_Edit)]
        public virtual async Task<CreateOrEditEmployeeSalaryDto> GetEmployeeSalaryForEdit(EntityDto<Guid> input)
        {
            var data = await _employeeSalaryRepository.FirstOrDefaultAsync(input.Id);

            var output = new CreateOrEditEmployeeSalaryDto
            {
                Id = data.Id,
                Month = data.Month,
                DateMiti = data.DateMiti,
                BasicSalary = data.BasicSalary,
                GradeAmount = data.GradeAmount,
                TechnicalAmount = data.TechnicalAmount,
                TotalGradeAmount = data.TotalGradeAmount,
                TotalBasicSalary = data.TotalBasicSalary,
                InsuranceAmount = data.InsuranceAmount,
                TotalSalary = data.TotalSalary,
                DearnessAllowance = data.DearnessAllowance,
                PrincipalAllowance = data.PrincipalAllowance,
                TotalWithAllowance = data.TotalWithAllowance,
                TotalMonth = data.TotalMonth,
                TotalSalaryAmount = data.TotalSalaryAmount,
                FestiableAllowance = data.FestiableAllowance,
                GovernmentAmount = data.GovernmentAmount,
                InternalAmount = data.InternalAmount,
                PaidSalaryAmount = data.PaidSalaryAmount,
                SchoolInfoId = data.SchoolInfoId,
                EmployeeId = data.EmployeeId,
                EmployeeLevelId = data.EmployeeLevelId
            };


            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditEmployeeSalaryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_EmployeeSalary_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeSalaryDto input)
        {
            var employeeSalary = new EmployeeSalary
            {
                Month = input.Month,
                DateMiti = input.DateMiti,
                BasicSalary = input.BasicSalary,
                GradeAmount = input.GradeAmount,
                TechnicalAmount = input.TechnicalAmount,
                TotalGradeAmount = input.TotalGradeAmount,
                TotalBasicSalary = input.TotalBasicSalary,
                InsuranceAmount = input.InsuranceAmount,
                TotalSalary = input.TotalSalary,
                DearnessAllowance = input.DearnessAllowance,
                PrincipalAllowance = input.PrincipalAllowance,
                TotalWithAllowance = input.TotalWithAllowance,
                TotalMonth = input.TotalMonth,
                TotalSalaryAmount = input.TotalSalaryAmount,
                FestiableAllowance = input.FestiableAllowance,
                GovernmentAmount = input.GovernmentAmount,
                InternalAmount = input.InternalAmount,
                PaidSalaryAmount = input.PaidSalaryAmount,
                SchoolInfoId = input.SchoolInfoId,
                EmployeeId = input.EmployeeId,
                EmployeeLevelId = input.EmployeeLevelId,
                TenantId = AbpSession.TenantId
            };


            await _employeeSalaryRepository.InsertAsync(employeeSalary);
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeSalary_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeSalaryDto input)
        {
            var employeeSalary = await _employeeSalaryRepository.FirstOrDefaultAsync(e => e.Id == input.Id);
            if (employeeSalary == null)
                throw new UserFriendlyException("Data not found");

            employeeSalary.Month = input.Month;
            employeeSalary.DateMiti = input.DateMiti;
            employeeSalary.BasicSalary = input.BasicSalary;
            employeeSalary.GradeAmount = input.GradeAmount;
            employeeSalary.TechnicalAmount = input.TechnicalAmount;
            employeeSalary.TotalGradeAmount = input.TotalGradeAmount;
            employeeSalary.TotalBasicSalary = input.TotalBasicSalary;
            employeeSalary.InsuranceAmount = input.InsuranceAmount;
            employeeSalary.TotalSalary = input.TotalSalary;
            employeeSalary.DearnessAllowance = input.DearnessAllowance;
            employeeSalary.PrincipalAllowance = input.PrincipalAllowance;
            employeeSalary.TotalWithAllowance = input.TotalWithAllowance;
            employeeSalary.TotalMonth = input.TotalMonth;
            employeeSalary.TotalSalaryAmount = input.TotalSalaryAmount;
            employeeSalary.FestiableAllowance = input.FestiableAllowance;
            employeeSalary.GovernmentAmount = input.GovernmentAmount;
            employeeSalary.InternalAmount = input.InternalAmount;
            employeeSalary.PaidSalaryAmount = input.PaidSalaryAmount;
            employeeSalary.SchoolInfoId = input.SchoolInfoId;
            employeeSalary.EmployeeId = input.EmployeeId;
            employeeSalary.EmployeeLevelId = input.EmployeeLevelId;
            employeeSalary.TenantId = AbpSession.TenantId;
            await _employeeSalaryRepository.UpdateAsync(employeeSalary);
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeSalary_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            await _employeeSalaryRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetEmployeeSalaryToExcel(GetAllEmployeeSalaryForExcelInput input)
        {
            var monthFilter = input.MonthFilter.HasValue
                ? (Months)input.MonthFilter
                : default;

            var filteredEmployeeSalary = _employeeSalaryRepository.GetAll()
                .Include(e => e.SchoolInfoFk)
                .Include(e => e.EmployeeFk)
                .Include(e => e.EmployeeLevelFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DateMiti.Contains(input.Filter))
                .WhereIf(input.MonthFilter.HasValue && input.MonthFilter > -1, e => e.Month == monthFilter)
                .WhereIf(input.MinTechnicalAmountFilter != null,
                    e => e.TechnicalAmount >= input.MinTechnicalAmountFilter)
                .WhereIf(input.MaxTechnicalAmountFilter != null,
                    e => e.TechnicalAmount <= input.MaxTechnicalAmountFilter)
                .WhereIf(input.MinTotalGradeAmountFilter != null,
                    e => e.TotalGradeAmount >= input.MinTotalGradeAmountFilter)
                .WhereIf(input.MaxTotalGradeAmountFilter != null,
                    e => e.TotalGradeAmount <= input.MaxTotalGradeAmountFilter)
                .WhereIf(input.MinInsuranceAmountFilter != null,
                    e => e.InsuranceAmount >= input.MinInsuranceAmountFilter)
                .WhereIf(input.MaxInsuranceAmountFilter != null,
                    e => e.InsuranceAmount <= input.MaxInsuranceAmountFilter)
                .WhereIf(input.MinTotalSalaryFilter != null, e => e.TotalSalary >= input.MinTotalSalaryFilter)
                .WhereIf(input.MaxTotalSalaryFilter != null, e => e.TotalSalary <= input.MaxTotalSalaryFilter)
                .WhereIf(input.MinDearnessAllowanceFilter != null,
                    e => e.DearnessAllowance >= input.MinDearnessAllowanceFilter)
                .WhereIf(input.MaxDearnessAllowanceFilter != null,
                    e => e.DearnessAllowance <= input.MaxDearnessAllowanceFilter)
                .WhereIf(input.MinTotalWithAllowanceFilter != null,
                    e => e.TotalWithAllowance >= input.MinTotalWithAllowanceFilter)
                .WhereIf(input.MaxTotalWithAllowanceFilter != null,
                    e => e.TotalWithAllowance <= input.MaxTotalWithAllowanceFilter)
                .WhereIf(input.MinGovernmentAmountFilter != null,
                    e => e.GovernmentAmount >= input.MinGovernmentAmountFilter)
                .WhereIf(input.MaxGovernmentAmountFilter != null,
                    e => e.GovernmentAmount <= input.MaxGovernmentAmountFilter)
                .WhereIf(input.MinInternalAmountFilter != null, e => e.InternalAmount >= input.MinInternalAmountFilter)
                .WhereIf(input.MaxInternalAmountFilter != null, e => e.InternalAmount <= input.MaxInternalAmountFilter)
                .WhereIf(input.MinPaidSalaryAmountFilter != null,
                    e => e.PaidSalaryAmount >= input.MinPaidSalaryAmountFilter)
                .WhereIf(input.MaxPaidSalaryAmountFilter != null,
                    e => e.PaidSalaryAmount <= input.MaxPaidSalaryAmountFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SchoolInfoNameFilter),
                    e => e.SchoolInfoFk != null && e.SchoolInfoFk.Name == input.SchoolInfoNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter),
                    e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeLevelNameFilter),
                    e => e.EmployeeLevelFk != null && e.EmployeeLevelFk.Name == input.EmployeeLevelNameFilter);

            var query = (from o in filteredEmployeeSalary
                join o1 in _lookupSchoolInfoRepository.GetAll() on o.SchoolInfoId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookupEmployeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _lookupEmployeeLevelRepository.GetAll() on o.EmployeeLevelId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                select new GetEmployeeSalaryForViewDto
                {
                    Month = o.Month.ToString(),
                    DateMiti = o.DateMiti,
                    BasicSalary = o.BasicSalary,
                    GradeAmount = o.GradeAmount,
                    TechnicalAmount = o.TechnicalAmount,
                    TotalGradeAmount = o.TotalGradeAmount,
                    TotalBasicSalary = o.TotalBasicSalary,
                    InsuranceAmount = o.InsuranceAmount,
                    TotalSalary = o.TotalSalary,
                    DearnessAllowance = o.DearnessAllowance,
                    PrincipalAllowance = o.PrincipalAllowance,
                    TotalWithAllowance = o.TotalWithAllowance,
                    TotalMonth = o.TotalMonth,
                    TotalSalaryAmount = o.TotalSalaryAmount,
                    FestiableAllowance = o.FestiableAllowance,
                    GovernmentAmount = o.GovernmentAmount,
                    InternalAmount = o.InternalAmount,
                    PaidSalaryAmount = o.PaidSalaryAmount,
                    Id = o.Id,

                    SchoolInfoName = s1 == null || s1.Name == null ? "" : s1.Name,
                    EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name,
                    EmployeeLevelName = s3 == null || s3.Name == null ? "" : s3.Name
                });

            var employeeSalaryListDtos = await query.ToListAsync();

            return _employeeSalaryExcelExporter.ExportToFile(employeeSalaryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeSalary)]
        public async Task<List<EmployeeSalarySchoolInfoLookupTableDto>> GetAllSchoolInfoForTableDropdown()
        {
            return await _lookupSchoolInfoRepository.GetAll()
                .Select(schoolInfo => new EmployeeSalarySchoolInfoLookupTableDto
                {
                    Id = schoolInfo.Id.ToString(),
                    DisplayName = schoolInfo == null || schoolInfo.Name == null ? "" : schoolInfo.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeSalary)]
        public async Task<List<EmployeeSalaryEmployeeLookupTableDto>> GetAllEmployeeForTableDropdown()
        {
            return await _lookupEmployeeRepository.GetAll()
                .Select(employee => new EmployeeSalaryEmployeeLookupTableDto
                {
                    Id = employee.Id.ToString(),
                    DisplayName = employee == null || employee.Name == null ? "" : employee.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeSalary)]
        public async Task<List<EmployeeSalaryEmployeeLevelLookupTableDto>> GetAllEmployeeLevelForTableDropdown()
        {
            return await _lookupEmployeeLevelRepository.GetAll()
                .Select(employeeLevel => new EmployeeSalaryEmployeeLevelLookupTableDto
                {
                    Id = employeeLevel.Id.ToString(),
                    DisplayName = employeeLevel == null || employeeLevel.Name == null
                        ? ""
                        : employeeLevel.Name.ToString()
                }).ToListAsync();
        }
    }
}