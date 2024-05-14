using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
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
    [AbpAuthorize(AppPermissions.Pages_GradeUpgrades)]
    public class GradeUpgradesAppService : PayrollAppServiceBase, IGradeUpgradesAppService
    {
        private readonly IRepository<GradeUpgrade, Guid> _gradeUpgradeRepository;
        private readonly IGradeUpgradesExcelExporter _gradeUpgradesExcelExporter;
        private readonly IRepository<Employee, Guid> _lookup_employeeRepository;

        public GradeUpgradesAppService(IRepository<GradeUpgrade, Guid> gradeUpgradeRepository, IGradeUpgradesExcelExporter gradeUpgradesExcelExporter, IRepository<Employee, Guid> lookup_employeeRepository)
        {
            _gradeUpgradeRepository = gradeUpgradeRepository;
            _gradeUpgradesExcelExporter = gradeUpgradesExcelExporter;
            _lookup_employeeRepository = lookup_employeeRepository;

        }

        public virtual async Task<PagedResultDto<GetGradeUpgradeForViewDto>> GetAll(GetAllGradeUpgradesInput input)
        {
            var gradeFilter = input.GradeFilter.HasValue
                        ? (EmployeeGrade)input.GradeFilter
                        : default;

            var filteredGradeUpgrades = _gradeUpgradeRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DateMiti.Contains(input.Filter) || e.Remarks.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DateMitiFilter), e => e.DateMiti.Contains(input.DateMitiFilter))
                        .WhereIf(input.GradeFilter.HasValue && input.GradeFilter > -1, e => e.Grade == gradeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks.Contains(input.RemarksFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var pagedAndFilteredGradeUpgrades = filteredGradeUpgrades
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var gradeUpgrades = from o in pagedAndFilteredGradeUpgrades
                                join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                select new
                                {

                                    o.DateMiti,
                                    o.Grade,
                                    o.Remarks,
                                    Id = o.Id,
                                    EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                };

            var totalCount = await filteredGradeUpgrades.CountAsync();

            var dbList = await gradeUpgrades.ToListAsync();
            var results = new List<GetGradeUpgradeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetGradeUpgradeForViewDto()
                {

                    DateMiti = o.DateMiti,
                    Grade = o.Grade,
                    Remarks = o.Remarks,
                    Id = o.Id,
                    EmployeeName = o.EmployeeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetGradeUpgradeForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetGradeUpgradeForViewDto> GetGradeUpgradeForView(Guid id)
        {
            var gradeUpgrade = await _gradeUpgradeRepository.GetAsync(id);

            var output = new GetGradeUpgradeForViewDto
            {
                DateMiti = gradeUpgrade.DateMiti,
                Grade = gradeUpgrade.Grade,
                Remarks = gradeUpgrade.Remarks,
                Id = id,
                EmployeeId = gradeUpgrade.EmployeeId
            };

            if (output.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((Guid)output.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GradeUpgrades_Edit)]
        public virtual async Task<GetGradeUpgradeForEditOutput> GetGradeUpgradeForEdit(EntityDto<Guid> input)
        {
            var gradeUpgrade = await _gradeUpgradeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGradeUpgradeForEditOutput
            {
                EmployeeId = gradeUpgrade.EmployeeId,
                DateMiti = gradeUpgrade?.DateMiti,
                Grade = gradeUpgrade.Grade,
                Remarks = gradeUpgrade.Remarks,
                Id = input.Id,
            };

            if (output.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((Guid)output.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditGradeUpgradeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_GradeUpgrades_Create)]
        protected virtual async Task Create(CreateOrEditGradeUpgradeDto input)
        {
            var gradeUpgrade = new GradeUpgrade
            {
                DateMiti = input.DateMiti,
                Remarks = input.Remarks,
                EmployeeId = input.EmployeeId,
                Grade = input.Grade,
                IsActive = true,
            };

            if (AbpSession.TenantId != null)
            {
                gradeUpgrade.TenantId = (int?)AbpSession.TenantId;
            }

            await _gradeUpgradeRepository.InsertAsync(gradeUpgrade);

        }

        [AbpAuthorize(AppPermissions.Pages_GradeUpgrades_Edit)]
        protected virtual async Task Update(CreateOrEditGradeUpgradeDto input)
        {
            var gradeUpgrade = await _gradeUpgradeRepository.FirstOrDefaultAsync((Guid)input.Id);
            if (gradeUpgrade != null)
            {
                gradeUpgrade.Remarks = input.Remarks;
                gradeUpgrade.Grade = input.Grade;
                gradeUpgrade.DateMiti = input.DateMiti;
                gradeUpgrade.EmployeeId = input.EmployeeId;
                gradeUpgrade.IsActive = false;
                await _gradeUpgradeRepository.UpdateAsync(gradeUpgrade);
                await Create(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_GradeUpgrades_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            await _gradeUpgradeRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetGradeUpgradesToExcel(GetAllGradeUpgradesForExcelInput input)
        {
            var gradeFilter = input.GradeFilter.HasValue
                        ? (EmployeeGrade)input.GradeFilter
                        : default;

            var filteredGradeUpgrades = _gradeUpgradeRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DateMiti.Contains(input.Filter) || e.Remarks.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DateMitiFilter), e => e.DateMiti.Contains(input.DateMitiFilter))
                        .WhereIf(input.GradeFilter.HasValue && input.GradeFilter > -1, e => e.Grade == gradeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks.Contains(input.RemarksFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var query = (from o in filteredGradeUpgrades
                         join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetGradeUpgradeForViewDto()
                         {
                             DateMiti = o.DateMiti,
                             Grade = o.Grade,
                             Remarks = o.Remarks,
                             Id = o.Id,
                             EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var gradeUpgradeListDtos = await query.ToListAsync();

            return _gradeUpgradesExcelExporter.ExportToFile(gradeUpgradeListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_GradeUpgrades)]
        public async Task<List<GradeUpgradeEmployeeLookupTableDto>> GetAllEmployeeForTableDropdown()
        {
            return await _lookup_employeeRepository.GetAll()
                .Select(employee => new GradeUpgradeEmployeeLookupTableDto
                {
                    Id = employee.Id.ToString(),
                    DisplayName = employee == null || employee.Name == null ? "" : employee.Name.ToString()
                }).ToListAsync();
        }

    }
}