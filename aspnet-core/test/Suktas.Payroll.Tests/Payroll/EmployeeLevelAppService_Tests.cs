using System;
using System.Linq;
using System.Threading.Tasks;
using Suktas.Payroll.Payroll.Dtos;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Suktas.Payroll.Payroll;
using Shouldly;
using Xunit;
using Abp.Timing;

namespace Suktas.Payroll.Tests.Payroll
{
    public class EmployeeLevelsAppService_Tests : AppTestBase
    {
        private readonly IEmployeeLevelsAppService _employeeLevelsAppService;

        private readonly Guid _employeeLevelTestId;

        public EmployeeLevelsAppService_Tests()
        {
            _employeeLevelsAppService = Resolve<IEmployeeLevelsAppService>();
            _employeeLevelTestId = Guid.NewGuid();
            SeedTestData();
        }

        private void SeedTestData()
        {
            var currentTenant = GetCurrentTenant();

            var employeeLevel = new EmployeeLevel
            {
                Name = "Test value",
                Id = _employeeLevelTestId,
                TenantId = currentTenant.Id
            };

            UsingDbContext(context =>
            {
                context.EmployeeLevels.Add(employeeLevel);
            });
        }

        [Fact]
        public async Task Should_Get_All_EmployeeLevels()
        {
            var employeeLevels = await _employeeLevelsAppService.GetAll(new GetAllEmployeeLevelsInput());

            employeeLevels.TotalCount.ShouldBe(1);
            employeeLevels.Items.Count.ShouldBe(1);

            employeeLevels.Items.Any(x => x.EmployeeLevel.Id == _employeeLevelTestId).ShouldBe(true);
        }

        [Fact]
        public async Task Should_Get_EmployeeLevel_For_View()
        {
            var employeeLevel = await _employeeLevelsAppService.GetEmployeeLevelForView(_employeeLevelTestId);

            employeeLevel.ShouldNotBe(null);
        }

        [Fact]
        public async Task Should_Get_EmployeeLevel_For_Edit()
        {
            var employeeLevel = await _employeeLevelsAppService.GetEmployeeLevelForEdit(new EntityDto<Guid> { Id = _employeeLevelTestId });

            employeeLevel.ShouldNotBe(null);
        }

        [Fact]
        protected virtual async Task Should_Create_EmployeeLevel()
        {
            var employeeLevel = new CreateOrEditEmployeeLevelDto
            {
                Name = "Test value",
                Id = _employeeLevelTestId
            };

            await _employeeLevelsAppService.CreateOrEdit(employeeLevel);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.EmployeeLevels.FirstOrDefaultAsync(e => e.Id == _employeeLevelTestId);
                entity.ShouldNotBe(null);
            });
        }

        [Fact]
        protected virtual async Task Should_Update_EmployeeLevel()
        {
            var employeeLevel = new CreateOrEditEmployeeLevelDto
            {
                Name = "Updated test value",
                Id = _employeeLevelTestId
            };

            await _employeeLevelsAppService.CreateOrEdit(employeeLevel);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.EmployeeLevels.FirstOrDefaultAsync(e => e.Id == employeeLevel.Id);
                entity.ShouldNotBeNull();

                entity.Name.ShouldBe("Updated test value");
            });
        }

        [Fact]
        public async Task Should_Delete_EmployeeLevel()
        {
            await _employeeLevelsAppService.Delete(new EntityDto<Guid> { Id = _employeeLevelTestId });

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.EmployeeLevels.FirstOrDefaultAsync(e => e.Id == _employeeLevelTestId);
                entity.ShouldBeNull();
            });
        }
    }
}