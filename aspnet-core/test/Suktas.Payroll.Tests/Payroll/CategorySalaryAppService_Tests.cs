using Suktas.Payroll.Municipality.Enum;

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
    public class CategorySalaryAppService_Tests : AppTestBase
    {
        private readonly ICategorySalaryAppService _categorySalaryAppService;

        private readonly Guid _categorySalaryTestId;

        public CategorySalaryAppService_Tests()
        {
            _categorySalaryAppService = Resolve<ICategorySalaryAppService>();
            _categorySalaryTestId = Guid.NewGuid();
            SeedTestData();
        }

        private void SeedTestData()
        {
            var currentTenant = GetCurrentTenant();

            var categorySalary = new CategorySalary
            {
                Salary = 0,
                Category = 0,
                TechnicalAmount = 0,
                Id = _categorySalaryTestId,
                TenantId = currentTenant.Id
            };

            UsingDbContext(context =>
            {
                context.CategorySalary.Add(categorySalary);
            });
        }

        [Fact]
        public async Task Should_Get_All_CategorySalary()
        {
            var categorySalary = await _categorySalaryAppService.GetAll(new GetAllCategorySalaryInput());

            categorySalary.TotalCount.ShouldBe(1);
            categorySalary.Items.Count.ShouldBe(1);

            categorySalary.Items.Any(x => x.CategorySalary.Id == _categorySalaryTestId).ShouldBe(true);
        }

        [Fact]
        public async Task Should_Get_CategorySalary_For_View()
        {
            var categorySalary = await _categorySalaryAppService.GetCategorySalaryForView(_categorySalaryTestId);

            categorySalary.ShouldNotBe(null);
        }

        [Fact]
        public async Task Should_Get_CategorySalary_For_Edit()
        {
            var categorySalary = await _categorySalaryAppService.GetCategorySalaryForEdit(new EntityDto<Guid> { Id = _categorySalaryTestId });

            categorySalary.ShouldNotBe(null);
        }

        [Fact]
        protected virtual async Task Should_Create_CategorySalary()
        {
            var categorySalary = new CreateOrEditCategorySalaryDto
            {
                Salary = 0,
                Category = 0,
                TechnicalAmount = 0,
                Id = _categorySalaryTestId
            };

            await _categorySalaryAppService.CreateOrEdit(categorySalary);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.CategorySalary.FirstOrDefaultAsync(e => e.Id == _categorySalaryTestId);
                entity.ShouldNotBe(null);
            });
        }

        [Fact]
        protected virtual async Task Should_Update_CategorySalary()
        {
            var categorySalary = new CreateOrEditCategorySalaryDto
            {
                Salary = 1,
                Category = 0,
                TechnicalAmount = 1,
                Id = _categorySalaryTestId
            };

            await _categorySalaryAppService.CreateOrEdit(categorySalary);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.CategorySalary.FirstOrDefaultAsync(e => e.Id == categorySalary.Id);
                entity.ShouldNotBeNull();

                entity.Salary.ShouldBe(1);
                entity.Category.ShouldBe((EmployeeCategory)0);
                entity.TechnicalAmount.ShouldBe(1);
            });
        }

        [Fact]
        public async Task Should_Delete_CategorySalary()
        {
            await _categorySalaryAppService.Delete(new EntityDto<Guid> { Id = _categorySalaryTestId });

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.CategorySalary.FirstOrDefaultAsync(e => e.Id == _categorySalaryTestId);
                entity.ShouldBeNull();
            });
        }
    }
}