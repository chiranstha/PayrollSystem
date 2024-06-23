using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Suktas.Payroll.Authorization;
using Suktas.Payroll.Payroll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    [AbpAuthorize(AppPermissions.Pages_MonthlyAllowance)]

    public class MonthlyAllowanceAppService : PayrollAppServiceBase, IMonthlyAllowanceAppService
    {
        private readonly IRepository<MonthlyAllowances, Guid> _monthlyAllowancesRepository;

        public MonthlyAllowanceAppService(IRepository<MonthlyAllowances, Guid> monthlyAllowancesRepository)
        {
            _monthlyAllowancesRepository = monthlyAllowancesRepository;
        }

        public async Task<List<CreateOrEditMontlyAllowanceDto>> GetAll()
        {
            var result = new List<CreateOrEditMontlyAllowanceDto>();
            var data = await _monthlyAllowancesRepository.GetAll().ToListAsync();
            foreach (var item in data)
            {
                result.Add(new CreateOrEditMontlyAllowanceDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Amount = item.Amount,
                    EmployeeCategory = item.EmployeeCategory
                });
            }
            return result;
        }

        [AbpAuthorize(AppPermissions.Pages_MonthlyAllowance_Create)]
        public async Task CreateOrEdit(CreateOrEditMontlyAllowanceDto input)
        {
            if (input.Id == Guid.Empty|| input.Id==null)
                await Create(input);
            else
                await Update(input);
        }


        [AbpAuthorize(AppPermissions.Pages_MonthlyAllowance_Create)]
        public async Task Create(CreateOrEditMontlyAllowanceDto input)
        {
            var data = new MonthlyAllowances
            {
                Name = input.Name,
                Amount = input.Amount,
                EmployeeCategory = input.EmployeeCategory,
                TenantId = AbpSession.GetTenantId()
            };
            await _monthlyAllowancesRepository.InsertAsync(data);
        }

        [AbpAuthorize(AppPermissions.Pages_MonthlyAllowance_Edit)]

        public async Task Update(CreateOrEditMontlyAllowanceDto input)
        {
            var data = await _monthlyAllowancesRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (data != null)
            {
                data.Name = input.Name;
                data.Amount = input.Amount;
                data.EmployeeCategory = input.EmployeeCategory;
                await _monthlyAllowancesRepository.UpdateAsync(data);
            }
            else
                throw new UserFriendlyException("Data not found");
        }

        [AbpAuthorize(AppPermissions.Pages_MonthlyAllowance_Edit)]
        public virtual async Task<GetMonthlyAllowanceForEdit> GetMonthlyAllowanceForEdit(EntityDto<Guid> input)
        {
            var data = await _monthlyAllowancesRepository.GetAll().AsNoTracking()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (data == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetMonthlyAllowanceForEdit
            {
                Id = data.Id,
                Amount = data.Amount,
                Name = data.Name,
                EmployeeCategory = data.EmployeeCategory
            };

            return output;
        }
        [AbpAuthorize(AppPermissions.Pages_MonthlyAllowance_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            var employeeLevel = await _monthlyAllowancesRepository.GetAll()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (employeeLevel == null)
                throw new UserFriendlyException("Data not found");

            await _monthlyAllowancesRepository.DeleteAsync(employeeLevel);
        }
    }
}
