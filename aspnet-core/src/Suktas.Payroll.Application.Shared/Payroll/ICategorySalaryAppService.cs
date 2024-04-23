using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using System.Collections.Generic;

namespace Suktas.Payroll.Payroll
{
    public interface ICategorySalaryAppService : IApplicationService
    {
        Task<PagedResultDto<GetCategorySalaryForViewDto>> GetAll(GetAllCategorySalaryInput input);

        Task<GetCategorySalaryForViewDto> GetCategorySalaryForView(Guid id);

        Task<GetCategorySalaryForEditOutput> GetCategorySalaryForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditCategorySalaryDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetCategorySalaryToExcel(GetAllCategorySalaryForExcelInput input);

        Task<List<CategorySalaryEmployeeLevelLookupTableDto>> GetAllEmployeeLevelForTableDropdown();

    }
}