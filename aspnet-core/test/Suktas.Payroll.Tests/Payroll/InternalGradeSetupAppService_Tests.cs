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
    public class InternalGradeSetupAppService_Tests : AppTestBase
    {
        private readonly IInternalGradeSetupAppService _internalGradeSetupAppService;

        private readonly Guid _internalGradeSetupTestId;

        public InternalGradeSetupAppService_Tests()
        {
            _internalGradeSetupAppService = Resolve<IInternalGradeSetupAppService>();
            _internalGradeSetupTestId = Guid.NewGuid();
            SeedTestData();
        }

        private void SeedTestData()
        {
            var currentTenant = GetCurrentTenant();

            var internalGradeSetup = new InternalGradeSetup
            {
                Category = 0,
                Grade = 0,
                IsPercent = false,
                Value = 0,
                Id = _internalGradeSetupTestId,
                TenantId = currentTenant.Id
            };

            UsingDbContext(context =>
            {
                context.InternalGradeSetup.Add(internalGradeSetup);
            });
        }

        [Fact]
        public async Task Should_Get_All_InternalGradeSetup()
        {
            var internalGradeSetup = await _internalGradeSetupAppService.GetAll(new GetAllInternalGradeSetupInput());

            internalGradeSetup.TotalCount.ShouldBe(1);
            internalGradeSetup.Items.Count.ShouldBe(1);

           }

        [Fact]
        public async Task Should_Get_InternalGradeSetup_For_View()
        {
            var internalGradeSetup = await _internalGradeSetupAppService.GetInternalGradeSetupForView(_internalGradeSetupTestId);

            internalGradeSetup.ShouldNotBe(null);
        }

        [Fact]
        public async Task Should_Get_InternalGradeSetup_For_Edit()
        {
            var internalGradeSetup = await _internalGradeSetupAppService.GetInternalGradeSetupForEdit(new EntityDto<Guid> { Id = _internalGradeSetupTestId });

            internalGradeSetup.ShouldNotBe(null);
        }

        [Fact]
        protected virtual async Task Should_Create_InternalGradeSetup()
        {
            var internalGradeSetup = new CreateOrEditInternalGradeSetupDto
            {
                Category = 0,
                Grade = 0,
                IsPercent = false,
                Value = 0,
                Id = _internalGradeSetupTestId
            };

            await _internalGradeSetupAppService.CreateOrEdit(internalGradeSetup);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.InternalGradeSetup.FirstOrDefaultAsync(e => e.Id == _internalGradeSetupTestId);
                entity.ShouldNotBe(null);
            });
        }

        [Fact]
        protected virtual async Task Should_Update_InternalGradeSetup()
        {
            var internalGradeSetup = new CreateOrEditInternalGradeSetupDto
            {
                Category = 0,
                Grade = 0,
                IsPercent = true,
                Value = 1,
                Id = _internalGradeSetupTestId
            };

            await _internalGradeSetupAppService.CreateOrEdit(internalGradeSetup);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.InternalGradeSetup.FirstOrDefaultAsync(e => e.Id == internalGradeSetup.Id);
                entity.ShouldNotBeNull();

                entity.Category.ShouldBe((EmployeeCategory)0);
                entity.Grade.ShouldBe((EmployeeGrade)0);
                entity.IsPercent.ShouldBe(true);
                entity.Value.ShouldBe(1);
            });
        }

        [Fact]
        public async Task Should_Delete_InternalGradeSetup()
        {
            await _internalGradeSetupAppService.Delete(new EntityDto<Guid> { Id = _internalGradeSetupTestId });

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.InternalGradeSetup.FirstOrDefaultAsync(e => e.Id == _internalGradeSetupTestId);
                entity.ShouldBeNull();
            });
        }
    }
}