using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.Record.PivotTable;
using Suktas.Payroll.Municipality.Enum;
using Suktas.Payroll.Payroll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Messaging.V1;

namespace Suktas.Payroll.Payroll
{
    public class ReportsAppService : PayrollAppServiceBase
    {
        private readonly IRepository<EmployeeSalaryMasterNew, Guid> _employeeSalaryMasterNew;
        private readonly IRepository<EmployeeSalaryDetailNew, Guid> _employeeSalaryDetailNewRepository;
        private readonly IRepository<EmployeeLevel, Guid> _employeeLevelRepository;
        private readonly IRepository<EmployeeSalaryMasterMonthNew, Guid> _employeeSalaryMasterMonthNew;
        private readonly IRepository<SchoolInfo, Guid> _lookupSchoolInfoRepository;
        private readonly IRepository<Employee, Guid> _employeeRepository;

        public ReportsAppService(IRepository<EmployeeSalaryDetailNew, Guid> employeeSalaryDetailRepository,
            IRepository<EmployeeLevel, Guid> employeeLevelRepository,
            IRepository<SchoolInfo, Guid> lookupSchoolInfoRepository,
            IRepository<Employee, Guid> employeeRepository,
            IRepository<EmployeeSalaryMasterMonthNew, Guid> employeeSalaryMasterMonthNew,
            IRepository<EmployeeSalaryMasterNew, Guid> employeeSalaryMasterRepository)
        {
            _employeeSalaryMasterNew = employeeSalaryMasterRepository;
            _employeeLevelRepository = employeeLevelRepository;
            _lookupSchoolInfoRepository = lookupSchoolInfoRepository;
            _employeeSalaryMasterMonthNew = employeeSalaryMasterMonthNew;
            _employeeRepository = employeeRepository;
            _employeeSalaryDetailNewRepository = employeeSalaryDetailRepository;
        }


        public virtual async Task<List<LevelWiseReportDto>> GetLevelWiseReport(int year, Guid level)
        {
            var result = new List<LevelWiseReportDto>();
            var master = await _employeeSalaryMasterNew.GetAll().Where(x => x.Year == year).ToListAsync();
            var masterIds = master.Select(x => x.Id).Distinct().ToList();
            var data = await _employeeSalaryDetailNewRepository.GetAll()
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


        public virtual async Task<List<SchoolWiseReportDto>> GetCategoryWiseReport(int year, Guid schoolId,CategoryEnum category)
        {
            var result = new List<SchoolWiseReportDto>();
        //    var data = await _employeeSalaryDetailNewRepository.GetAll().Where(x => x.SchoolInfoId == schoolId).ToListAsync();
            var master = _employeeSalaryMasterNew.GetAll().Where(x => x.Year == year);         
            var masterIds = master.Select(x => x.Id).Distinct();
            var months = await _employeeSalaryMasterMonthNew.GetAll().Where(x => masterIds.Contains(x.EmployeeSalaryMasterNewId)).ToListAsync();

            var details = await _employeeSalaryDetailNewRepository.GetAll().Where(x => masterIds.Contains(x.EmployeeSalaryMasterNewId) && x.SchoolInfoId == schoolId).ToListAsync();
            foreach(var masterId in masterIds)
            {
                decimal total = 0;

                var monthCount = months.Where(x => x.EmployeeSalaryMasterNewId == masterId).Count();
                switch (category)
                {
                    case CategoryEnum.BasicSalry:
                        total = details.Where(x => x.EmployeeSalaryMasterNewId == masterId).Sum(x => x.BasicSalary)* monthCount;
                        break;
                    case CategoryEnum.GradeAmount:
                        total = details.Where(x => x.EmployeeSalaryMasterNewId == masterId).Sum(x => x.GradeAmount) * monthCount;
                        break;
                    case CategoryEnum.TechnicalGradeAmount:
                        total = details.Where(x => x.EmployeeSalaryMasterNewId == masterId).Sum(x => x.TechnicalGradeAmount) * monthCount;
                        break;
                    case CategoryEnum.EPFAmount:
                        total = details.Where(x => x.EmployeeSalaryMasterNewId == masterId).Sum(x => x.EPFAmount) * monthCount;
                        break;
                    case CategoryEnum.InsuranceAmount:
                        total = details.Where(x => x.EmployeeSalaryMasterNewId == masterId).Sum(x => x.InsuranceAmount) * monthCount;
                        break;
                    case CategoryEnum.TotalSalry:
                        total = details.Where(x => x.EmployeeSalaryMasterNewId == masterId).Sum(x => x.TotalSalary) * monthCount;
                        break;
                    case CategoryEnum.InflationAmount:
                        total = details.Where(x => x.EmployeeSalaryMasterNewId == masterId).Sum(x => x.InflationAllowance) * monthCount;
                        break;
                    case CategoryEnum.PrincipalAllowance:
                        total = details.Where(x => x.EmployeeSalaryMasterNewId == masterId).Sum(x => x.PrincipalAllowance) * monthCount;
                        break;
                    case CategoryEnum.FestivalAllowance:
                        total = details.Where(x => x.EmployeeSalaryMasterNewId == masterId).Sum(x => x.FestivalAllowance);
                        break;
                    default:
                        total = 0;
                        break;
                }
                var datum = new SchoolWiseReportDto
                {
                    Year = year,
                    Months = string.Join(',', months.Where(x => x.EmployeeSalaryMasterNewId == masterId).Select(x => x.Month).Select(x => x.ToString())),
                    TotalAmount = total,
                };
                result.Add(datum);
            }
            
            return result;
        }
        public async Task<List<EmployeeSalarySchoolInfoLookupTableDto>> GetAllSchoolInfoForTableDropdown()
        {
            return await _lookupSchoolInfoRepository.GetAll()
                .Select(schoolInfo => new EmployeeSalarySchoolInfoLookupTableDto
                {
                    Id = schoolInfo.Id,
                    DisplayName = schoolInfo == null || schoolInfo.Name == null ? "" : schoolInfo.Name.ToString()
                }).ToListAsync();
        }

        public async Task<List<PhaseWiseReportDto>> PhaseWiseReport(int year)
        {
            var result = new List<PhaseWiseReportDto>();
            var data = await _employeeSalaryMasterNew.GetAll().Where(x => x.Year == year && x.TenantId == AbpSession.TenantId).ToListAsync();
            foreach(var datum in data)
            {
                var schools = await _employeeSalaryDetailNewRepository.GetAll()
                    .Where(x => x.EmployeeSalaryMasterNewId == datum.Id)
                    .Select(x => x.SchoolInfoId).Distinct().ToListAsync();
                var school = await _lookupSchoolInfoRepository.GetAll().Where(x => schools.Contains(x.Id)).Select(x => x.Name).ToListAsync();
                var dat = new PhaseWiseReportDto
                {
                    Months = datum.Months,
                    Schools = string.Join(",", school),
                    TotalAmount = datum.TotalAmount,
                    DueAmount = datum.DueAmount,
                    ExtraAmount = datum.ExtraAmount,
                    FinalAmount = datum.FinalAmount,
                    Remarks = datum.Remarks,
                };
                result.Add(dat);
            }
            return result;
        }

        public async Task<List<TeacherWiseReportDto>> TeacherWiseReport(int year, Guid teacherId)
        {
            var data = await _employeeSalaryDetailNewRepository.GetAll().Include(x => x.EmployeeSalaryMasterNewFk)
                .Where(x => x.EmployeeId == teacherId && x.EmployeeSalaryMasterNewFk.Year == year)
                .Select(x => new TeacherWiseReportDto
                {
                    BasicSalary = x.BasicSalary,
                    Grade = x.Grade,
                    GradeAmount = x.GradeAmount,
                    TechnicalGradeAmount = x.TechnicalGradeAmount,
                    TotalGradeAmount = x.TotalGradeAmount,
                    Total = x.Total,
                    EPFAmount = x.EPFAmount,
                    InflationAllowance = x.InflationAllowance,
                    InsuranceAmount = x.InsuranceAmount,
                    TotalSalary = x.TotalSalary,
                    InternalAmount = x.InternalAmount,
                    FestivalAllowance = x.FestivalAllowance,
                    PrincipalAllowance = x.PrincipalAllowance,
                    TotalForAllMonths = x.TotalForAllMonths,
                    TotalPaidAmount = x.TotalPaidAmount,
                    TotalSalaryAmount = x.TotalSalaryAmount,
                    TotalWithAllowanceForAllMonths = x.TotalWithAllowanceForAllMonths,
                    GradeRate = x.GradeRate,
                    Month = x.Month,
                    MonthNames = x.MonthNames,
                }).ToListAsync();
            return data;
        }

        public async Task<List<EmployeeSalaryEmployeeLevelLookupTableDto>> GetAllEmployee()
        {
            return (await _employeeRepository.GetAll().Select(x => new EmployeeSalaryEmployeeLevelLookupTableDto
            {
                Id = x.Id,
                DisplayName = x.Name
            }).ToListAsync());
        }
    }
}
 