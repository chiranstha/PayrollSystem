using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Suktas.Payroll.Payroll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    public class ReportsAppService : PayrollAppServiceBase
    {
        private readonly IRepository<EmployeeSalaryMasterNew, Guid> _employeeSalaryMasterRepository;
        private readonly IRepository<EmployeeSalaryDetailNew, Guid> _employeeSalaryDetailRepository;
        private readonly IRepository<EmployeeLevel, Guid> _employeeLevelRepository;

        public ReportsAppService(IRepository<EmployeeSalaryDetailNew, Guid> employeeSalaryDetailRepository,
            IRepository<EmployeeLevel, Guid> employeeLevelRepository,
            IRepository<EmployeeSalaryMasterNew, Guid> employeeSalaryMasterRepository)
        {
            _employeeSalaryMasterRepository = employeeSalaryMasterRepository;
            _employeeLevelRepository = employeeLevelRepository;
            _employeeSalaryDetailRepository = employeeSalaryDetailRepository;
        }


        public virtual async Task<List<LevelWiseReportDto>> GetLevelWiseReport(int year, Guid level)
        {
            var result = new List<LevelWiseReportDto>();
            var master = await _employeeSalaryMasterRepository.GetAll().Where(x => x.Year == year).ToListAsync();
            var masterIds = master.Select(x => x.Id).Distinct().ToList();
            var data = await _employeeSalaryDetailRepository.GetAll()
                .Include(x => x.EmployeeFk).Include(x => x.SchoolInfoFk)
                .Where(x => x.EmployeeFk.EmployeeLevelId == level && masterIds.Contains(x.EmployeeSalaryMasterNewId)).ToListAsync();

            foreach(var dat in data)
            {
                result.Add(new LevelWiseReportDto
                {
                    School = dat.SchoolInfoFk.Name,
                    EmpName = dat.EmployeeName,
                    Months = dat.MonthNames,
                    Salary = dat.TotalPaidAmount
                });
            }
            return result;
        }

        public virtual async Task<List<EmployeeSalaryEmployeeLevelLookupTableDto>> GetAllLevels()
        {
            return await _employeeLevelRepository.GetAll().Select(x => new EmployeeSalaryEmployeeLevelLookupTableDto
            {
                Id = x.Id,
                DisplayName = x.Name
            }).ToListAsync();
        }

    }
}
