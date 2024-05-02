using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Suktas.Payroll.DataExporting.Excel.NPOI;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Storage;

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
                            sheet,  employeeSalary,
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
    }
}