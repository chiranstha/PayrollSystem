using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Suktas.Payroll.Authorization;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Payroll.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    [AbpAuthorize(AppPermissions.Pages_PrincipalAllowanceSettings)]
    public class PrincipalAllowanceSettingsAppService : PayrollAppServiceBase, IPrincipalAllowanceSettingsAppService
    {
        private readonly IRepository<PrincipalAllowanceSetting, Guid> _PrincipalAllowanceSettingRepository;
        private readonly IPrincipalAllowanceSettingsExcelExporter _PrincipalAllowanceSettingsExcelExporter;
        private readonly IRepository<EmployeeLevel, Guid> _lookup_employeeLevelRepository;

        public PrincipalAllowanceSettingsAppService(IRepository<PrincipalAllowanceSetting, Guid> PrincipalAllowanceSettingRepository, IPrincipalAllowanceSettingsExcelExporter PrincipalAllowanceSettingsExcelExporter, IRepository<EmployeeLevel, Guid> lookup_employeeLevelRepository)
        {
            _PrincipalAllowanceSettingRepository = PrincipalAllowanceSettingRepository;
            _PrincipalAllowanceSettingsExcelExporter = PrincipalAllowanceSettingsExcelExporter;
            _lookup_employeeLevelRepository = lookup_employeeLevelRepository;

        }

        public virtual async Task<PagedResultDto<GetPrincipalAllowanceSettingForViewDto>> GetAll(GetAllPrincipalAllowanceSettingsInput input)
        {

            var filteredPrincipalAllowanceSettings = _PrincipalAllowanceSettingRepository.GetAll()
                        .Include(e => e.EmployeeLevelFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeLevelNameFilter), e => e.EmployeeLevelFk != null && e.EmployeeLevelFk.Name == input.EmployeeLevelNameFilter);

            var pagedAndFilteredPrincipalAllowanceSettings = filteredPrincipalAllowanceSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var PrincipalAllowanceSettings = from o in pagedAndFilteredPrincipalAllowanceSettings
                                             join o1 in _lookup_employeeLevelRepository.GetAll() on o.EmployeeLevelId equals o1.Id into j1
                                             from s1 in j1.DefaultIfEmpty()

                                             select new
                                             {

                                                 o.Amount,
                                                 Id = o.Id,
                                                 EmployeeLevelName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                             };

            var totalCount = await filteredPrincipalAllowanceSettings.CountAsync();

            var dbList = await PrincipalAllowanceSettings.ToListAsync();
            var results = new List<GetPrincipalAllowanceSettingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPrincipalAllowanceSettingForViewDto()
                {
                    Amount = o.Amount,
                    Id = o.Id,
                    EmployeeLevelName = o.EmployeeLevelName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPrincipalAllowanceSettingForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetPrincipalAllowanceSettingForViewDto> GetPrincipalAllowanceSettingForView(Guid id)
        {
            var PrincipalAllowanceSetting = await _PrincipalAllowanceSettingRepository.GetAsync(id);

            var output = new GetPrincipalAllowanceSettingForViewDto
            {
                Amount = PrincipalAllowanceSetting.Amount,
                EmployeeLevelId = PrincipalAllowanceSetting.EmployeeLevelId
            };

            if (output.EmployeeLevelId != null)
            {
                var _lookupEmployeeLevel = await _lookup_employeeLevelRepository.FirstOrDefaultAsync((Guid)output.EmployeeLevelId);
                output.EmployeeLevelName = _lookupEmployeeLevel?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PrincipalAllowanceSettings_Edit)]
        public virtual async Task<GetPrincipalAllowanceSettingForEditOutput> GetPrincipalAllowanceSettingForEdit(EntityDto<Guid> input)
        {
            var PrincipalAllowanceSetting = await _PrincipalAllowanceSettingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPrincipalAllowanceSettingForEditOutput
            {
                Id = PrincipalAllowanceSetting?.Id,
                Amount = PrincipalAllowanceSetting.Amount,
                EmployeeLevelId = PrincipalAllowanceSetting.EmployeeLevelId
            };

            if (output.EmployeeLevelId != null)
            {
                var _lookupEmployeeLevel = await _lookup_employeeLevelRepository.FirstOrDefaultAsync((Guid)output.EmployeeLevelId);
                output.EmployeeLevelName = _lookupEmployeeLevel?.Name?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditPrincipalAllowanceSettingDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_PrincipalAllowanceSettings_Create)]
        protected virtual async Task Create(CreateOrEditPrincipalAllowanceSettingDto input)
        {
            var PrincipalAllowanceSetting = ObjectMapper.Map<PrincipalAllowanceSetting>(input);

            if (AbpSession.TenantId != null)
            {
                PrincipalAllowanceSetting.TenantId = (int?)AbpSession.TenantId;
            }

            await _PrincipalAllowanceSettingRepository.InsertAsync(PrincipalAllowanceSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_PrincipalAllowanceSettings_Edit)]
        protected virtual async Task Update(CreateOrEditPrincipalAllowanceSettingDto input)
        {
            var PrincipalAllowanceSetting = await _PrincipalAllowanceSettingRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, PrincipalAllowanceSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_PrincipalAllowanceSettings_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            await _PrincipalAllowanceSettingRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetPrincipalAllowanceSettingsToExcel(GetAllPrincipalAllowanceSettingsForExcelInput input)
        {

            var filteredPrincipalAllowanceSettings = _PrincipalAllowanceSettingRepository.GetAll()
                        .Include(e => e.EmployeeLevelFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeLevelNameFilter), e => e.EmployeeLevelFk != null && e.EmployeeLevelFk.Name == input.EmployeeLevelNameFilter);

            var query = (from o in filteredPrincipalAllowanceSettings
                         join o1 in _lookup_employeeLevelRepository.GetAll() on o.EmployeeLevelId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetPrincipalAllowanceSettingForViewDto()
                         {
                             Amount = o.Amount,
                             Id = o.Id,
                             EmployeeLevelName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var PrincipalAllowanceSettingListDtos = await query.ToListAsync();

            return _PrincipalAllowanceSettingsExcelExporter.ExportToFile(PrincipalAllowanceSettingListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_PrincipalAllowanceSettings)]
        public async Task<List<PrincipalAllowanceSettingEmployeeLevelLookupTableDto>> GetAllEmployeeLevelForTableDropdown()
        {
            return await _lookup_employeeLevelRepository.GetAll()
                .Select(employeeLevel => new PrincipalAllowanceSettingEmployeeLevelLookupTableDto
                {
                    Id = employeeLevel.Id.ToString(),
                    DisplayName = employeeLevel == null || employeeLevel.Name == null ? "" : employeeLevel.Name.ToString()
                }).ToListAsync();
        }

    }
}