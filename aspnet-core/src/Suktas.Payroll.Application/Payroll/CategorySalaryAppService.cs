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
    [AbpAuthorize(AppPermissions.Pages_CategorySalary)]
    public class CategorySalaryAppService : PayrollAppServiceBase, ICategorySalaryAppService
    {
        private readonly IRepository<CategorySalary, Guid> _categorySalaryRepository;
        private readonly ICategorySalaryExcelExporter _categorySalaryExcelExporter;
        private readonly IRepository<EmployeeLevel, Guid> _lookupEmployeeLevelRepository;

        public CategorySalaryAppService(IRepository<CategorySalary, Guid> categorySalaryRepository,
            ICategorySalaryExcelExporter categorySalaryExcelExporter,
            IRepository<EmployeeLevel, Guid> lookupEmployeeLevelRepository)
        {
            _categorySalaryRepository = categorySalaryRepository;
            _categorySalaryExcelExporter = categorySalaryExcelExporter;
            _lookupEmployeeLevelRepository = lookupEmployeeLevelRepository;

        }

        public virtual async Task<PagedResultDto<GetCategorySalaryForViewDto>> GetAll(GetAllCategorySalaryInput input)
        {

            var filteredCategorySalary = _categorySalaryRepository.GetAll()
                        .Include(e => e.EmployeeLevelFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e =>  e.EmployeeLevelFk.Name.Contains(input.Filter));

            var pagedAndFilteredCategorySalary = filteredCategorySalary
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var categorySalary = from o in pagedAndFilteredCategorySalary
                                 join o1 in _lookupEmployeeLevelRepository.GetAll() on o.EmployeeLevelId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 select new
                                 {

                                     o.Salary,
                                     o.Category,
                                     o.TechnicalAmount,
                                     o.Id,
                                     EmployeeLevelName = s1 == null || s1.Name == null ? "" : s1.Name
                                 };

            var totalCount = await filteredCategorySalary.CountAsync();

            var dbList = await categorySalary.ToListAsync();
            var results = dbList.Select(o => new GetCategorySalaryForViewDto
                {
                    Salary = o.Salary,
                    Category = o.Category.ToString(),
                    TechnicalAmount = o.TechnicalAmount,
                    Id = o.Id,
                    EmployeeLevelName = o.EmployeeLevelName
                })
                .ToList();

            return new PagedResultDto<GetCategorySalaryForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetCategorySalaryForViewDto> GetCategorySalaryForView(Guid id)
        {
            var categorySalary = await _categorySalaryRepository.GetAll().AsNoTracking()
                .Include(x => x.EmployeeLevelFk)
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == id);

            if (categorySalary == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetCategorySalaryForViewDto
            {
                Id = categorySalary.Id,
                Salary = categorySalary.Salary,
                Category = categorySalary.Category.ToString(),
                TechnicalAmount = categorySalary.TechnicalAmount,
                EmployeeLevelName = categorySalary.EmployeeLevelFk.Name
            };

            
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CategorySalary_Edit)]
        public virtual async Task<GetCategorySalaryForEditOutput> GetCategorySalaryForEdit(EntityDto<Guid> input)
        {
            var categorySalary = await _categorySalaryRepository.GetAll().AsNoTracking()
                .Include(x => x.EmployeeLevelFk)
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            if (categorySalary == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetCategorySalaryForEditOutput
            {
                Id = categorySalary.Id,
                Salary = categorySalary.Salary,
                Category = categorySalary.Category,
                TechnicalAmount = categorySalary.TechnicalAmount,
                EmployeeLevelId = categorySalary.EmployeeLevelId
            };


            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCategorySalaryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CategorySalary_Create)]
        protected virtual async Task Create(CreateOrEditCategorySalaryDto input)
        {
            var categorySalary = new CategorySalary
            {
                Id = Guid.Empty,
                TenantId = AbpSession.GetTenantId(),
                Salary = input.Salary,
                Category = input.Category,
                TechnicalAmount = input.TechnicalAmount,
                EmployeeLevelId = input.EmployeeLevelId
            };

            await _categorySalaryRepository.InsertAsync(categorySalary);

        }

        [AbpAuthorize(AppPermissions.Pages_CategorySalary_Edit)]
        protected virtual async Task Update(CreateOrEditCategorySalaryDto input)
        {
            var categorySalary = await _categorySalaryRepository.GetAll().Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
           if (categorySalary == null)
                throw new UserFriendlyException("Data not found");

            categorySalary.Salary = input.Salary;
            categorySalary.Category = input.Category;
            categorySalary.TechnicalAmount = input.TechnicalAmount;
            categorySalary.EmployeeLevelId = input.EmployeeLevelId;

            await _categorySalaryRepository.UpdateAsync(categorySalary);
        }

        [AbpAuthorize(AppPermissions.Pages_CategorySalary_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            var categorySalary = await _categorySalaryRepository.GetAll().Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (categorySalary == null)
                throw new UserFriendlyException("Data not found");
            await _categorySalaryRepository.DeleteAsync(categorySalary.Id);
        }

        public virtual async Task<FileDto> GetCategorySalaryToExcel(GetAllCategorySalaryForExcelInput input)
        {

            var filteredCategorySalary = _categorySalaryRepository.GetAll()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .Include(e => e.EmployeeLevelFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.EmployeeLevelFk.Name.Contains(input.Filter));

            var query = (from o in filteredCategorySalary
                         join o1 in _lookupEmployeeLevelRepository.GetAll() on o.EmployeeLevelId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetCategorySalaryForViewDto
                         {
                             Salary = o.Salary,
                             Category = o.Category.ToString(),
                             TechnicalAmount = o.TechnicalAmount,
                             Id = o.Id,
                             EmployeeLevelName = s1 == null || s1.Name == null ? "" : s1.Name
                         });

            var categorySalaryListDtos = await query.ToListAsync();

            return _categorySalaryExcelExporter.ExportToFile(categorySalaryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_CategorySalary)]
        public async Task<List<CategorySalaryEmployeeLevelLookupTableDto>> GetAllEmployeeLevelForTableDropdown()
        {
            return await _lookupEmployeeLevelRepository.GetAll()
                .Select(employeeLevel => new CategorySalaryEmployeeLevelLookupTableDto
                {
                    Id = employeeLevel.Id,
                    DisplayName = employeeLevel == null || employeeLevel.Name == null ? "" : employeeLevel.Name.ToString()
                }).ToListAsync();
        }

    }
}