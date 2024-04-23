using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;

namespace Suktas.Payroll.Payroll
{
    public interface IEmployeeLevelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetEmployeeLevelForView>> GetAll(GetAllEmployeeLevelsInput input);

        Task<GetEmployeeLevelForView> GetEmployeeLevelForView(Guid id);

        Task<GetEmployeeLevelForEdit> GetEmployeeLevelForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditEmployeeLevelDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetEmployeeLevelsToExcel(GetAllEmployeeLevelsForExcelInput input);

    }
}