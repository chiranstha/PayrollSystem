using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
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
    [AbpAuthorize(AppPermissions.Pages_EmployeeSalary)]
    public class EmployeeSalaryAppService : PayrollAppServiceBase, IEmployeeSalaryAppService
    {
        private readonly IRepository<EmployeeSalary, Guid> _employeeSalaryRepository;
        private readonly IEmployeeSalaryExcelExporter _employeeSalaryExcelExporter;
        private readonly IRepository<SchoolInfo, Guid> _lookupSchoolInfoRepository;
        private readonly IRepository<Employee, Guid> _lookupEmployeeRepository;
        private readonly IRepository<EmployeeLevel, Guid> _lookupEmployeeLevelRepository;
        private readonly IRepository<CategorySalary, Guid> _categorySalaryRepository;
        private readonly IRepository<EmployeeSalaryMasterNew, Guid> _employeeSalaryMasterNew;
        private readonly IRepository<GradeUpgrade, Guid> _gradeUpgradeRepository;
        private readonly IRepository<EmployeeSalaryMasterSchoolNew, Guid> _employeeSalaryMasterSchoolNew;
        private readonly IRepository<EmployeeSalaryMasterMonthNew, Guid> _employeeSalaryMasterMonthNew;
        private readonly IRepository<FestivalBonusSetting, Guid> _festivalBonusSettingRepository;
        private readonly IRepository<PrincipalAllowanceSetting, Guid> _principalAllowanceSettingRepository;
        private readonly IRepository<EmployeeSalaryDetailNew, Guid> _employeeSalaryDetailNewRepository;
        private readonly IRepository<MonthlyAllowances, Guid> _monthlyAllowancesRepository;
        public EmployeeSalaryAppService(IRepository<EmployeeSalary, Guid> employeeSalaryRepository,
            IEmployeeSalaryExcelExporter employeeSalaryExcelExporter,
            IRepository<FestivalBonusSetting, Guid> festivalBonusSettingRepository,
            IRepository<EmployeeSalaryMasterNew, Guid> employeeSalaryMasterNew,
            IRepository<GradeUpgrade, Guid> gradeUpgradeRepository,
            IRepository<EmployeeSalaryDetailNew, Guid> employeeSalaryDetailNewRepository,
            IRepository<MonthlyAllowances, Guid> monthlyAllowancesRepository,
            IRepository<EmployeeSalaryMasterMonthNew, Guid> employeeSalaryMasterMonthNew,
            IRepository<EmployeeSalaryMasterSchoolNew, Guid> employeeSalaryMasterSchoolNew,
            IRepository<PrincipalAllowanceSetting, Guid> principalAllowanceSettingRepository,
            IRepository<SchoolInfo, Guid> lookupSchoolInfoRepository,
            IRepository<CategorySalary, Guid> categorySalaryRepository,
            IRepository<Employee, Guid> lookupEmployeeRepository,
            IRepository<EmployeeLevel, Guid> lookupEmployeeLevelRepository)
        {
            _employeeSalaryRepository = employeeSalaryRepository;
            _employeeSalaryExcelExporter = employeeSalaryExcelExporter;
            _festivalBonusSettingRepository = festivalBonusSettingRepository;
            _principalAllowanceSettingRepository = principalAllowanceSettingRepository;
            _lookupSchoolInfoRepository = lookupSchoolInfoRepository;
            _employeeSalaryDetailNewRepository = employeeSalaryDetailNewRepository;
            _categorySalaryRepository = categorySalaryRepository;
            _employeeSalaryMasterSchoolNew = employeeSalaryMasterSchoolNew;
            _lookupEmployeeRepository = lookupEmployeeRepository;
            _monthlyAllowancesRepository = monthlyAllowancesRepository;
            _employeeSalaryMasterMonthNew = employeeSalaryMasterMonthNew;
            _employeeSalaryMasterNew = employeeSalaryMasterNew;
            _gradeUpgradeRepository = gradeUpgradeRepository;
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


        public async Task<List<CreateOrEditEmployeeSalaryDto>> GenerateSalary(Guid schoolId, Months month)
        {
            var result = new List<CreateOrEditEmployeeSalaryDto>();
            var school = await _lookupSchoolInfoRepository.FirstOrDefaultAsync(x => x.Id == schoolId);
            var employees = await _lookupEmployeeRepository.GetAll().Where(x => x.SchoolInfoId == schoolId).ToListAsync();
            var categorySalaries = await _categorySalaryRepository.GetAll().ToListAsync();
            var gradeUpgrades = await _gradeUpgradeRepository.GetAll().ToListAsync();
            var principalAllowanceSettins = await _principalAllowanceSettingRepository.GetAll().ToListAsync();
            var festivalAllowance = await _festivalBonusSettingRepository.GetAll().ToListAsync();
            foreach (var employee in employees)
            {
                var data = new CreateOrEditEmployeeSalaryDto
                {
                    Month = month,
                    DateMiti = DateTime.Today.ToString(),
                    BasicSalary = categorySalaries.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId && x.Category == employee.Category) == null ? 0 : categorySalaries.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId && x.Category == employee.Category).Salary,
                    TechnicalAmount = employee.IsTechnical ? categorySalaries.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId && x.Category == employee.Category) == null ? 0 : categorySalaries.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId && x.Category == employee.Category).TechnicalAmount : 0,
                    TotalGradeAmount = 0,
                    TotalBasicSalary = 0,
                    InsuranceAmount = 0,
                    TotalSalary = 0,
                    DearnessAllowance = 0,
                    PrincipalAllowance = employee.IsPrincipal ? principalAllowanceSettins.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId) == null ? 0 : principalAllowanceSettins.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId).Amount : 0,
                    TotalWithAllowance = 0,
                    TotalMonth = 3,
                    TotalSalaryAmount = 45,
                    GovernmentAmount = 0,
                    InternalAmount = 0,
                    PaidSalaryAmount = 0,
                    SchoolInfoId = schoolId,
                    EmployeeId = employee.Id,
                    EmployeeLevelId = employee.EmployeeLevelId
                };
                data.FestiableAllowance = festivalAllowance.FirstOrDefault(x => x.MonthId == month) != null ? data.BasicSalary : 0;
                data.GradeAmount = (int)(gradeUpgrades.FirstOrDefault(x => x.EmployeeId == employee.Id).Grade) * data.BasicSalary / 30;

                result.Add(data);
            }
            return result;
        }

        public async Task<CreateEmployeeSalaryNewMasterDto> GenerateSalaryNew(CreateGenerateSalaryNewDto input)
        {
            var result = new List<CreateEmployeeSalaryNewDto>();
            var schools = await _lookupSchoolInfoRepository.GetAll().Where(x => input.SchoolIds.Contains(x.Id)).ToListAsync();
            var employees = await _lookupEmployeeRepository.GetAll().Where(x => input.SchoolIds.Contains(x.SchoolInfoId))
                .Include(x => x.SchoolInfoFk).ThenInclude(x => x.SchoolLevelFk).Include(x => x.EmployeeLevelFk).ToListAsync();
            var categorySalaries = await _categorySalaryRepository.GetAll().ToListAsync();
            var gradeUpgrades = await _gradeUpgradeRepository.GetAll().ToListAsync();
            var principalAllowanceSettins = await _principalAllowanceSettingRepository.GetAll().ToListAsync();
            var festivalAllowance = await _festivalBonusSettingRepository.GetAll().Where(x => input.Months.Contains(x.MonthId)).ToListAsync();
            var monthlyAllowances = await _monthlyAllowancesRepository.GetAll().ToListAsync();
            int sn = 1;
            foreach (var employee in employees)
            {
                // new added start                
                var employeeSalaryDetailNew = await _employeeSalaryDetailNewRepository.GetAll().Include(x => x.EmployeeSalaryMasterNewFk)
                    .Where(x => x.EmployeeId == employee.Id && x.EmployeeSalaryMasterNewFk.Year == input.Year)
                    .ToListAsync();
                var paidMonths = await _employeeSalaryMasterMonthNew.GetAll()
                    .Where(x => employeeSalaryDetailNew.Select(y => y.EmployeeSalaryMasterNewId).Contains(x.EmployeeSalaryMasterNewId)).ToListAsync();
                List<Months> unPaidMonths = new List<Months>();
                foreach(var toPay in input.Months)
                {
                    if (paidMonths.Select(x => x.Month).Contains(toPay))
                        continue;
                    else
                        unPaidMonths.Add(toPay);
                }
                // new added end
                var monthlyAllowance = monthlyAllowances.Where(x => x.EmployeeCategory == employee.Category).ToList();
                var data = new CreateEmployeeSalaryNewDto
                {
                    SN = sn++,
                    WardNo = employee.SchoolInfoFk.WardNo,
                    EmployeeId = employee.Id,
                    SchoolLevel = employee.SchoolInfoFk.SchoolLevelFk.Name,
                    SchoolInfoId = employee.SchoolInfoId,
                    SchoolName = employee.SchoolInfoFk.Name,
                    EmployeeType = employee.Category.ToString(),
                    EmployeeLevel = employee.EmployeeLevelFk.Name,
                    EmployeeName = employee.Name,
                    BasicSalary = categorySalaries.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId && x.Category == employee.Category) == null ? 0 : categorySalaries.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId && x.Category == employee.Category).Salary,
                    Grade = (int)gradeUpgrades.FirstOrDefault(x => x.EmployeeId == employee.Id && x.IsActive).Grade + (int)gradeUpgrades.FirstOrDefault(x => x.EmployeeId == employee.Id && x.IsActive).TechnicalGrade,
                    TechnicalGradeAmount = employee.IsTechnical ? categorySalaries.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId && x.Category == employee.Category) == null ? 0 : categorySalaries.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId && x.Category == employee.Category).TechnicalAmount : 0,
                    InsuranceAmount = employee.InsuranceAmount,
                    InflationAllowance = monthlyAllowance.Count > 0 ? monthlyAllowance.Sum(x => x.Amount) : 0,
                    PrincipalAllowance = employee.IsPrincipal ? principalAllowanceSettins.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId) == null ? 0 : principalAllowanceSettins.FirstOrDefault(x => x.EmployeeLevelId == employee.EmployeeLevelId).Amount : 0,
                    Month = unPaidMonths.Count,
                    MonthNames = string.Join(',', unPaidMonths.Select(x => x.ToString())),
                    InternalAmount = 0,
                };
                data.GradeRate = Math.Round(data.BasicSalary / 30, 0);
                data.GradeAmount = data.Grade * data.GradeRate;
                data.TotalGradeAmount = data.GradeAmount + data.TechnicalGradeAmount;
                data.Total = data.BasicSalary + data.TotalGradeAmount;
                data.FestivalAllowance = (decimal)(festivalAllowance.Where(x => x.PercentOrAmount == PercentOrAmount.Amount)?.Sum(x => x.Value));
                data.FestivalAllowance += (decimal)(festivalAllowance.Where(x => x.PercentOrAmount == PercentOrAmount.Percent)?.Sum(x => data.Total * x.Value / 100));
                data.EPFAmount = employee.AddEPF ? (data.Total / 10) : 0;
                data.TotalSalary = data.Total + data.EPFAmount + data.InsuranceAmount;
                data.TotalSalaryAmount = data.TotalSalary + data.InflationAllowance + data.PrincipalAllowance;
                data.TotalForAllMonths = data.TotalSalaryAmount * data.Month;
                data.TotalWithAllowanceForAllMonths = data.TotalForAllMonths + data.FestivalAllowance;
                data.TotalPaidAmount = data.TotalWithAllowanceForAllMonths + data.InternalAmount;
                result.Add(data);
            }
            var total = new CreateEmployeeSalaryNewDto
            {
                BasicSalary = result.Sum(x => x.BasicSalary),
                GradeAmount = result.Sum(x => x.GradeAmount),
                TechnicalGradeAmount = result.Sum(x => x.TechnicalGradeAmount),
                TotalGradeAmount = result.Sum(x => x.TotalGradeAmount),
                Total = result.Sum(x => x.Total),
                EPFAmount = result.Sum(x => x.EPFAmount),
                InsuranceAmount = result.Sum(x => x.InsuranceAmount),
                TotalSalary = result.Sum(x => x.TotalSalary),
                InflationAllowance = result.Sum(x => x.InflationAllowance),
                PrincipalAllowance = result.Sum(x => x.PrincipalAllowance),
                TotalSalaryAmount = result.Sum(x => x.TotalSalaryAmount),
                TotalForAllMonths = result.Sum(x => x.TotalForAllMonths),
                FestivalAllowance = result.Sum(x => x.FestivalAllowance),
                TotalWithAllowanceForAllMonths = result.Sum(x => x.TotalWithAllowanceForAllMonths),
                InternalAmount = result.Sum(x => x.InternalAmount),
                TotalPaidAmount = result.Sum(x => x.TotalPaidAmount)
            };
            var masterResult = new CreateEmployeeSalaryNewMasterDto
            {
                Total = total,
                Details = result
            };
            return masterResult;
        }

        public async Task<List<MonthwiseReportDto>> GetAllSalaries(int year)
        {
            var result = new List<MonthwiseReportDto>();
            var query = _employeeSalaryMasterNew.GetAll();
            if (year != 0)
                query = query.Where(x => x.Year == year);
            var employeeSalaryMasters = await query.ToListAsync();
            var masterIds = employeeSalaryMasters.Select(x => x.Id).ToList();

            var detailQuery = _employeeSalaryDetailNewRepository.GetAll();
            var data = await detailQuery.Where(x => masterIds.Contains(x.EmployeeSalaryMasterNewId))
                .Select(x => new
                {
                    x.SchoolInfoId,
                    x.EmployeeSalaryMasterNewId,
                    x.TotalPaidAmount
                }).ToListAsync();
            foreach (var employeeSalaryMaster in employeeSalaryMasters)
            {
                var months = await _employeeSalaryMasterMonthNew.GetAll().Where(x => x.EmployeeSalaryMasterNewId == employeeSalaryMaster.Id)
                    .Select(x => x.Month).ToListAsync();
                var masterWiseData = data.Where(x => x.EmployeeSalaryMasterNewId == employeeSalaryMaster.Id);
                var distinctSchools = masterWiseData.Select(x => x.SchoolInfoId).Distinct();
                var schoolInfos = await _lookupSchoolInfoRepository.GetAll().ToListAsync();
                List<SchoolWithAmount> schoolWithAmounts = new List<SchoolWithAmount>();
                foreach (var distinctSchool in distinctSchools)
                {
                    schoolWithAmounts.Add(new SchoolWithAmount
                    {
                        SchoolNames = schoolInfos.FirstOrDefault(x => x.Id == distinctSchool).Name,
                        TotalAmount = masterWiseData.Where(x => x.SchoolInfoId == distinctSchool).Select(x => x.TotalPaidAmount).Sum()
                    });
                }
                var res = new MonthwiseReportDto
                {
                    Id = employeeSalaryMaster.Id,
                    Year = employeeSalaryMaster.Year,
                    MonthName = string.Join(',', months.Select(x => x.ToString())),
                    SchoolWithAmounts = schoolWithAmounts
                };
                result.Add(res);
            }
            return result;
        }

        public async Task<FileDto> GetAllSalariesExcel(int year)
        {
            var data = await GetAllSalaries(year);
            return _employeeSalaryExcelExporter.GetAllSalaries(data);
        }

        public async Task<List<SchoolWiseReportDto>> SchoolWiseReport(int year, Guid schoolId)
        {
            var result = new List<SchoolWiseReportDto>();
            var master = _employeeSalaryMasterNew.GetAll();
            if (year != 0)
                master = master.Where(x => x.Year == year);
            var masterIds = master.Select(x => x.Id).Distinct();
            var details = await _employeeSalaryDetailNewRepository.GetAll().Where(x => x.SchoolInfoId == schoolId && masterIds.Contains(x.EmployeeSalaryMasterNewId)).ToListAsync();
            var months = await _employeeSalaryMasterMonthNew.GetAll().Where(x => masterIds.Contains(x.EmployeeSalaryMasterNewId)).ToListAsync();
            foreach (var masterId in masterIds)
            {
                var data = new SchoolWiseReportDto
                {
                    Year = year,
                    Months = string.Join(',', months.Where(x => x.EmployeeSalaryMasterNewId == masterId).Select(x => x.Month).Select(x => x.ToString())),
                    TotalAmount = details.Where(x => x.EmployeeSalaryMasterNewId == masterId).Sum(x => x.TotalPaidAmount)
                };
                if (data.TotalAmount > 0)
                    result.Add(data);
            }
            return result;
        }

        public async Task<FileDto> SchoolWiseReportExcel(int year, Guid schoolId)
        {
            var data = await SchoolWiseReport(year,schoolId);
            return _employeeSalaryExcelExporter.SchoolWiseReport(data);
        }


        public async Task CreateSalaryNew(CreateSalaryNewDto data)
        {
            var employeeSalaryMaster = new EmployeeSalaryMasterNew
            {
                Year = data.Year,
                Months = string.Join(",", data.months.Select(x => x.ToString())),
                TotalAmount = data.TotalAmount,
                DueAmount = data.DueAmount,
                FinalAmount = data.FinalAmount,
                ExtraAmount = data.ExtraAmount,
                Remarks = data.Remarks,
                TenantId = AbpSession.TenantId
            };
            var masterId = await _employeeSalaryMasterNew.InsertAndGetIdAsync(employeeSalaryMaster);
            foreach (var month in data.months)
            {
                var employeeSalaryMasterMonthNew = new EmployeeSalaryMasterMonthNew
                {
                    TenantId = AbpSession.TenantId,
                    EmployeeSalaryMasterNewId = masterId,
                    Month = month
                };
                await _employeeSalaryMasterMonthNew.InsertAsync(employeeSalaryMasterMonthNew);
            }
            foreach (var detail in data.data)
            {
                var set = new EmployeeSalaryDetailNew
                {
                    SN = detail.SN,
                    WardNo = detail.WardNo,
                    EmployeeId = detail.EmployeeId,
                    SchoolLevel = detail.SchoolLevel,
                    SchoolName = detail.SchoolName,
                    EmployeeType = detail.EmployeeType,
                    EmployeeLevel = detail.EmployeeLevel,
                    EmployeeName = detail.EmployeeName,
                    BasicSalary = detail.BasicSalary,
                    Grade = detail.Grade,
                    GradeRate = detail.GradeRate,
                    GradeAmount = detail.GradeAmount,
                    TechnicalGradeAmount = detail.TechnicalGradeAmount,
                    TotalGradeAmount = detail.TotalGradeAmount,
                    Total = detail.Total,
                    EPFAmount = detail.EPFAmount,
                    InsuranceAmount = detail.InsuranceAmount,
                    TotalSalary = detail.TotalSalary,
                    InflationAllowance = detail.InflationAllowance,
                    PrincipalAllowance = detail.PrincipalAllowance,
                    TotalSalaryAmount = detail.TotalSalaryAmount,
                    Month = detail.Month,
                    MonthNames = detail.MonthNames,
                    TotalForAllMonths = detail.TotalForAllMonths,
                    FestivalAllowance = detail.FestivalAllowance,
                    TotalWithAllowanceForAllMonths = detail.TotalWithAllowanceForAllMonths,
                    InternalAmount = detail.InternalAmount,
                    TotalPaidAmount = detail.TotalPaidAmount,
                    Remarks = detail.Remarks,
                    IsPaid = true,
                    TenantId = AbpSession.TenantId,
                    SchoolInfoId = detail.SchoolInfoId,
                    EmployeeSalaryMasterNewId = masterId
                };
                await _employeeSalaryDetailNewRepository.InsertAsync(set);
            }
        }

        public async Task<CreateSalaryNewDto> GetSalaryNewForEdit(Guid masterId)
        {
            var result = new CreateSalaryNewDto
            {
                Year = (await _employeeSalaryMasterNew.FirstOrDefaultAsync(x => x.Id == masterId)).Year,
                //       schoolIds = await _employeeSalaryMasterSchoolNew.GetAll().Where(x => x.EmployeeSalaryMasterNewId == masterId).Select(x => x.SchoolInfoId).ToListAsync(),
                months = await _employeeSalaryMasterMonthNew.GetAll().Where(x => x.EmployeeSalaryMasterNewId == masterId).Select(x => x.Month).ToListAsync(),
                data = (await _employeeSalaryDetailNewRepository.GetAll().Where(x => x.EmployeeSalaryMasterNewId == masterId).ToListAsync()).Select(detail => new CreateEmployeeSalaryNewDto
                {
                    Id = detail.Id,
                    SN = detail.SN,
                    WardNo = detail.WardNo,
                    EmployeeId = detail.EmployeeId,
                    SchoolInfoId = detail.SchoolInfoId,
                    SchoolLevel = detail.SchoolLevel,
                    SchoolName = detail.SchoolName,
                    EmployeeType = detail.EmployeeType,
                    EmployeeLevel = detail.EmployeeLevel,
                    EmployeeName = detail.EmployeeName,
                    BasicSalary = detail.BasicSalary,
                    Grade = detail.Grade,
                    GradeRate = detail.GradeRate,
                    GradeAmount = detail.GradeAmount,
                    TechnicalGradeAmount = detail.TechnicalGradeAmount,
                    TotalGradeAmount = detail.TotalGradeAmount,
                    Total = detail.Total,
                    EPFAmount = detail.EPFAmount,
                    InsuranceAmount = detail.InsuranceAmount,
                    TotalSalary = detail.TotalSalary,
                    InflationAllowance = detail.InflationAllowance,
                    PrincipalAllowance = detail.PrincipalAllowance,
                    TotalSalaryAmount = detail.TotalSalaryAmount,
                    Month = detail.Month,
                    MonthNames = detail.MonthNames,
                    TotalForAllMonths = detail.TotalForAllMonths,
                    FestivalAllowance = detail.FestivalAllowance,
                    TotalWithAllowanceForAllMonths = detail.TotalWithAllowanceForAllMonths,
                    InternalAmount = detail.InternalAmount,
                    TotalPaidAmount = detail.TotalPaidAmount,
                    Remarks = detail.Remarks
                }).ToList(),
            };
            return result;
        }

        public async Task<FileDto> GenerateSalaryNewExcel(CreateGenerateSalaryNewDto input)
        {
            var data = await GenerateSalaryNew(input);
            return _employeeSalaryExcelExporter.ExportToFileSalary(data);
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
                    Id = schoolInfo.Id,
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
                    Id = employeeLevel.Id,
                    DisplayName = employeeLevel == null || employeeLevel.Name == null
                        ? ""
                        : employeeLevel.Name.ToString()
                }).ToListAsync();
        }
    }
}