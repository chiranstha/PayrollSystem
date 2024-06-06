using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Suktas.Payroll.DataExporting.Excel.NPOI;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Storage;
using System.Collections.Generic;

namespace Suktas.Payroll.Payroll.Exporting
{
    public class EmployeeSalaryExcelExporter : NpoiExcelExporterBase, IEmployeeSalaryExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeSalaryExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeSalaryForViewDto> employeeSalary)
        {
            return CreateExcelPackage(
                    "EmployeeSalary.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("EmployeeSalary"));

                        AddHeader(
                            sheet,
                        L("Month"),
                        L("DateMiti"),
                        L("BasicSalary"),
                        L("GradeAmount"),
                        L("TechnicalAmount"),
                        L("TotalGradeAmount"),
                        L("TotalBasicSalary"),
                        L("InsuranceAmount"),
                        L("TotalSalary"),
                        L("DearnessAllowance"),
                        L("PrincipalAllowance"),
                        L("TotalWithAllowance"),
                        L("TotalMonth"),
                        L("TotalSalaryAmount"),
                        L("FestiableAllowance"),
                        L("GovernmentAmount"),
                        L("InternalAmount"),
                        L("PaidSalaryAmount"),
                        (L("SchoolInfo")) + L("Name"),
                        (L("Employee")) + L("Name"),
                        (L("EmployeeLevel")) + L("Name")
                            );

                        AddObjects(
                            sheet, employeeSalary,
                        d => d.Month,
                        d => d.DateMiti,
                        d => d.BasicSalary,
                        d => d.GradeAmount,
                        d => d.TechnicalAmount,
                        d => d.TotalGradeAmount,
                        d => d.TotalBasicSalary,
                        d => d.InsuranceAmount,
                        d => d.TotalSalary,
                        d => d.DearnessAllowance,
                        d => d.PrincipalAllowance,
                        d => d.TotalWithAllowance,
                        d => d.TotalMonth,
                        d => d.TotalSalaryAmount,
                        d => d.FestiableAllowance,
                        d => d.GovernmentAmount,
                        d => d.InternalAmount,
                        d => d.PaidSalaryAmount,
                        d => d.SchoolInfoName,
                        d => d.EmployeeName,
                        d => d.EmployeeLevelName
                            );

                    });

        }

        public FileDto ExportToFileSalary(List<CreateEmployeeSalaryNewDto> data)
        {
            return CreateExcelPackage(
                    "EmployeeSalary.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet("EmployeeSalary");

                        AddHeader(
                            sheet,
                        "S.N.",
                        "Ward No",
                        "School Level",
                        "School Name",
                        "Employee Type",
                        "Employee Level",
                        "Employee Name",
                        "Basic Salary",
                        "Grade",
                        "Grade Rate",
                        "Grade Amount",
                        "Technical Grade Amount",
                        "Total Grade Amount",
                        "Total",
                        "EPF Amount",
                        "Insurance Amount",
                        "TotalSalary",
                        "Inflation Allowance",
                        "Principal Allowance",
                        "Total Salary Amount",
                        "Month",
                        "Month Names",
                        "Total For All Months",
                        "Festival Allowance",
                        "Total With Allowance For All Months",
                        "Internal Amount",
                        "Total Paid Amount",
                        "Remarks");

                        AddObjects(
                            sheet, data, d => d.SN,
                        d => d.WardNo,
                        d => d.SchoolLevel,
                        d => d.SchoolName,
                        d => d.EmployeeType,
                        d => d.EmployeeLevel,
                        d => d.EmployeeName,
                        d => d.BasicSalary,
                        d => d.Grade,
                        d => d.GradeRate,
                        d => d.GradeAmount,
                        d => d.TechnicalGradeAmount,
                        d => d.TotalGradeAmount,
                        d => d.Total,
                        d => d.EPFAmount,
                        d => d.InsuranceAmount,
                        d => d.TotalSalary,
                        d => d.InflationAllowance,
                        d => d.PrincipalAllowance,
                        d => d.TotalSalaryAmount,
                        d => d.Month,
                        d => d.MonthNames,
                        d => d.TotalForAllMonths,
                        d => d.FestivalAllowance,
                        d => d.TotalWithAllowanceForAllMonths,
                        d => d.InternalAmount,
                        d => d.TotalPaidAmount,
                        d => d.Remarks
                            );
                        ColumnResize(sheet, 28);

                    });
        }

        public FileDto GetAllSalaries(List<MonthwiseReportDto> data)
        {
            return CreateExcelPackage(
                   "Salary Report.xlsx",
                   excelPackage =>
                   {
                       var sheet = excelPackage.CreateSheet("Salary Report");

                       AddHeaderWithHeader(
                           sheet,2,
                       "S.N.",
                       "Year",
                       "MonthName");

                       //AddObjects(
                       //    sheet, data,
                       //    d => d.Id,
                       //    d => d.Year,
                       //    d => d.MonthName
                       //    );
                   });
        }

        public FileDto SchoolWiseReport(List<SchoolWiseReportDto> data)
        {
            return CreateExcelPackage(
                  "School Wise Report.xlsx",
                  excelPackage =>
                  {
                      var sheet = excelPackage.CreateSheet("School Wise Report");

                      AddHeader(sheet,
                      "Year",
                      "Month",
                      "Amount");

                      AddObjects(
                          sheet, data,
                          d => d.Year,
                          d => d.Months,
                          d => d.TotalAmount
                          );
                  });
        }
    }
}