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
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace Suktas.Payroll.Payroll
{
    [AbpAuthorize(AppPermissions.Pages_EmployeeLevels)]
    public  class EmployeeLevelsAppService : PayrollAppServiceBase, IEmployeeLevelsAppService
    {
        private readonly IRepository<CategorySalary, Guid> _categorySalaryRepository;
        private readonly IRepository<EmployeeLevel, Guid> _employeeLevelRepository;
        private readonly IEmployeeLevelsExcelExporter _employeeLevelsExcelExporter;

        public EmployeeLevelsAppService(
            IRepository<EmployeeLevel, Guid> employeeLevelRepository,
            IEmployeeLevelsExcelExporter employeeLevelsExcelExporter,
            IRepository<CategorySalary, Guid> categorySalaryRepository)
        {
            _employeeLevelRepository = employeeLevelRepository;
            _employeeLevelsExcelExporter = employeeLevelsExcelExporter;
            _categorySalaryRepository = categorySalaryRepository;
        }

        public virtual async Task<PagedResultDto<GetEmployeeLevelForView>> GetAll(GetAllEmployeeLevelsInput input)
        {

            var filteredEmployeeLevels = _employeeLevelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name.Contains(input.Filter));

            var pagedAndFilteredEmployeeLevels = filteredEmployeeLevels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employeeLevels = from o in pagedAndFilteredEmployeeLevels
                                 select new
                                 {

                                     o.Name,
                                     o.Id
                                 };

            var totalCount = await filteredEmployeeLevels.CountAsync();

            var dbList = await employeeLevels.ToListAsync();
            var results = new List<GetEmployeeLevelForView>();

            foreach (var o in dbList)
            {
                var res = new GetEmployeeLevelForView
                {
                        Name = o.Name,
                        Id = o.Id,
                };

                results.Add(res);
            }

            return new PagedResultDto<GetEmployeeLevelForView>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetEmployeeLevelForView> GetEmployeeLevelForView(Guid id)
        {
            var employeeLevel = await _employeeLevelRepository.GetAll().AsNoTracking()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == id);
            if (employeeLevel == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetEmployeeLevelForView
            {
                Id = employeeLevel.Id,
                Name = employeeLevel.Name
            };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeLevels_Edit)]
        public virtual async Task<GetEmployeeLevelForEdit> GetEmployeeLevelForEdit(EntityDto<Guid> input)
        {
            var employeeLevel = await _employeeLevelRepository.GetAll().AsNoTracking()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (employeeLevel == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetEmployeeLevelForEdit
            {
                Id = employeeLevel.Id,
                Name = employeeLevel.Name
            };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditEmployeeLevelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_EmployeeLevels_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeLevelDto input)
        {
            var employeeLevel = new EmployeeLevel
            {
                Id = Guid.Empty,
                TenantId = AbpSession.GetTenantId(),
                Name = input.Name,
            };

            await _employeeLevelRepository.InsertAsync(employeeLevel);

        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeLevels_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeLevelDto input)
        {
            var employeeLevel = await _employeeLevelRepository.GetAll()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (employeeLevel == null)
                throw new UserFriendlyException("Data not found");

            employeeLevel.Name = input.Name;
            await _employeeLevelRepository.UpdateAsync(employeeLevel);

        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeLevels_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            var employeeLevel = await _employeeLevelRepository.GetAll()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (employeeLevel == null)
                throw new UserFriendlyException("Data not found");

            if (await _categorySalaryRepository.GetAll().AnyAsync(x => x.EmployeeLevelId == input.Id))
                throw new UserFriendlyException("Can not Delete","Reference Exists in Category Salary");
            await _employeeLevelRepository.DeleteAsync(employeeLevel);
        }

        public virtual async Task<FileDto> GetEmployeeLevelsToExcel(GetAllEmployeeLevelsForExcelInput input)
        {

            var filteredEmployeeLevels = _employeeLevelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name.Contains(input.Filter));

            var query = (from o in filteredEmployeeLevels
                         select new GetEmployeeLevelForView
                         {
                                 Name = o.Name,
                                 Id = o.Id
                         });

            var employeeLevelListDtos = await query.ToListAsync();

            return _employeeLevelsExcelExporter.ExportToFile(employeeLevelListDtos);
        }

    }
}