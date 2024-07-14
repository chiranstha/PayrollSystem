using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Suktas.Payroll.Authorization;
using Suktas.Payroll.Payroll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    public class SchoolLevelAppService : PayrollAppServiceBase, ISchoolLevelAppService
    {
        private readonly IRepository<SchoolLevel, Guid> _SchoolLevelRepository;
        private readonly IRepository<CategorySalary, Guid> _categorySalaryRepository;

        public SchoolLevelAppService(IRepository<SchoolLevel, Guid> schoolLevelRepository, IRepository<CategorySalary, Guid> categorySalaryRepository)
        {
            _SchoolLevelRepository = schoolLevelRepository;
            _categorySalaryRepository = categorySalaryRepository;
        }
        public virtual async Task<PagedResultDto<GetSchoolLevelForView>> GetAll(GetAllSchoolLevelsInput input)
        {

            var filteredSchoolLevels = _SchoolLevelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name.Contains(input.Filter));

            var pagedAndFilteredSchoolLevels = filteredSchoolLevels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var SchoolLevels = from o in pagedAndFilteredSchoolLevels
                               select new
                               {
                                   o.AliasName,
                                   o.Name,
                                   o.Id
                               };

            var totalCount = await filteredSchoolLevels.CountAsync();

            var dbList = await SchoolLevels.ToListAsync();
            var results = new List<GetSchoolLevelForView>();

            foreach (var o in dbList)
            {
                var res = new GetSchoolLevelForView
                {
                    AliasName = o.AliasName,
                    Name = o.Name,
                    Id = o.Id,
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSchoolLevelForView>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetSchoolLevelForView> GetSchoolLevelForView(Guid id)
        {
            var SchoolLevel = await _SchoolLevelRepository.GetAll().AsNoTracking()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == id);
            if (SchoolLevel == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetSchoolLevelForView
            {
                Id = SchoolLevel.Id,
                Name = SchoolLevel.Name
            };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SchoolLevels_Edit)]
        public virtual async Task<GetSchoolLevelForEdit> GetSchoolLevelForEdit(EntityDto<Guid> input)
        {
            var SchoolLevel = await _SchoolLevelRepository.GetAll().AsNoTracking()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (SchoolLevel == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetSchoolLevelForEdit
            {
                Id = SchoolLevel.Id,
                AliasName = SchoolLevel.AliasName,
                Name = SchoolLevel.Name
            };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditSchoolLevelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SchoolLevels_Create)]
        protected virtual async Task Create(CreateOrEditSchoolLevelDto input)
        {
            var SchoolLevel = new SchoolLevel
            {
                Id = Guid.Empty,
                AliasName = input.AliasName,
                TenantId = AbpSession.GetTenantId(),
                Name = input.Name,
            };

            await _SchoolLevelRepository.InsertAsync(SchoolLevel);

        }

        [AbpAuthorize(AppPermissions.Pages_SchoolLevels_Edit)]
        protected virtual async Task Update(CreateOrEditSchoolLevelDto input)
        {
            var SchoolLevel = await _SchoolLevelRepository.GetAll()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (SchoolLevel == null)
                throw new UserFriendlyException("Data not found");

            SchoolLevel.Name = input.Name;
            SchoolLevel.AliasName = input.AliasName;
            await _SchoolLevelRepository.UpdateAsync(SchoolLevel);

        }

        [AbpAuthorize(AppPermissions.Pages_SchoolLevels_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            var SchoolLevel = await _SchoolLevelRepository.GetAll()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (SchoolLevel == null)
                throw new UserFriendlyException("Data not found");

            //if (await _categorySalaryRepository.GetAll().AnyAsync(x => x.EmployeeLevelId == input.Id))
            //throw new UserFriendlyException("Can not Delete", "Reference Exists in Category Salary");
            await _SchoolLevelRepository.DeleteAsync(SchoolLevel);
        }

        //public virtual async Task<FileDto> GetSchoolLevelsToExcel(GetAllSchoolLevelsForExcelInput input)
        //{

        //    var filteredSchoolLevels = _SchoolLevelRepository.GetAll()
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name.Contains(input.Filter));

        //    var query = (from o in filteredSchoolLevels
        //                 select new GetSchoolLevelForView
        //                 {
        //                     Name = o.Name,
        //                     Id = o.Id
        //                 });

        //    var SchoolLevelListDtos = await query.ToListAsync();

        //    return _SchoolLevelsExcelExporter.ExportToFile(SchoolLevelListDtos);
        //}



    }
}
