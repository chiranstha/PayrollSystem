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
    public class SchoolInfosAppService_Tests : AppTestBase
    {
        private readonly ISchoolInfosAppService _schoolInfosAppService;

        private readonly Guid _schoolInfoTestId;

        public SchoolInfosAppService_Tests()
        {
            _schoolInfosAppService = Resolve<ISchoolInfosAppService>();
            _schoolInfoTestId = Guid.NewGuid();
            SeedTestData();
        }

        private void SeedTestData()
        {
            var currentTenant = GetCurrentTenant();

            var schoolInfo = new SchoolInfo
            {
                Name = "Test value",
                Address = "Test value",
                PhoneNo = "Test value",
                Email = "Test value",
                Id = _schoolInfoTestId,
                TenantId = currentTenant.Id
            };

            UsingDbContext(context =>
            {
                context.SchoolInfos.Add(schoolInfo);
            });
        }

        [Fact]
        public async Task Should_Get_All_SchoolInfos()
        {
            var schoolInfos = await _schoolInfosAppService.GetAll(new GetAllSchoolInfosInput());

            schoolInfos.TotalCount.ShouldBe(1);
            schoolInfos.Items.Count.ShouldBe(1);

            schoolInfos.Items.Any(x => x.SchoolInfo.Id == _schoolInfoTestId).ShouldBe(true);
        }

        [Fact]
        public async Task Should_Get_SchoolInfo_For_View()
        {
            var schoolInfo = await _schoolInfosAppService.GetSchoolInfoForView(_schoolInfoTestId);

            schoolInfo.ShouldNotBe(null);
        }

        [Fact]
        public async Task Should_Get_SchoolInfo_For_Edit()
        {
            var schoolInfo = await _schoolInfosAppService.GetSchoolInfoForEdit(new EntityDto<Guid> { Id = _schoolInfoTestId });

            schoolInfo.ShouldNotBe(null);
        }

        [Fact]
        protected virtual async Task Should_Create_SchoolInfo()
        {
            var schoolInfo = new CreateOrEditSchoolInfoDto
            {
                Name = "Test value",
                Address = "Test value",
                PhoneNo = "Test value",
                Email = "Test value",
                Id = _schoolInfoTestId
            };

            await _schoolInfosAppService.CreateOrEdit(schoolInfo);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.SchoolInfos.FirstOrDefaultAsync(e => e.Id == _schoolInfoTestId);
                entity.ShouldNotBe(null);
            });
        }

        [Fact]
        protected virtual async Task Should_Update_SchoolInfo()
        {
            var schoolInfo = new CreateOrEditSchoolInfoDto
            {
                Name = "Updated test value",
                Address = "Updated test value",
                PhoneNo = "Updated test value",
                Email = "Updated test value",
                Id = _schoolInfoTestId
            };

            await _schoolInfosAppService.CreateOrEdit(schoolInfo);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.SchoolInfos.FirstOrDefaultAsync(e => e.Id == schoolInfo.Id);
                entity.ShouldNotBeNull();

                entity.Name.ShouldBe("Updated test value");
                entity.Address.ShouldBe("Updated test value");
                entity.PhoneNo.ShouldBe("Updated test value");
                entity.Email.ShouldBe("Updated test value");
            });
        }

        [Fact]
        public async Task Should_Delete_SchoolInfo()
        {
            await _schoolInfosAppService.Delete(new EntityDto<Guid> { Id = _schoolInfoTestId });

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.SchoolInfos.FirstOrDefaultAsync(e => e.Id == _schoolInfoTestId);
                entity.ShouldBeNull();
            });
        }
    }
}