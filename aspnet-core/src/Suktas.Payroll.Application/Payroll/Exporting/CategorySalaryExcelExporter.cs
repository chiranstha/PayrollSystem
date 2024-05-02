using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Suktas.Payroll.DataExporting.Excel.NPOI;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Storage;

namespace Suktas.Payroll.Payroll.Exporting
{
    public class CategorySalaryExcelExporter : NpoiExcelExporterBase, ICategorySalaryExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CategorySalaryExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCategorySalaryForViewDto> categorySalary)
        {
            return CreateExcelPackage(
                    "CategorySalary.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("CategorySalary"));

                        AddHeader(
                            sheet,
                        L("Salary"),
                        L("Category"),
                        L("TechnicalAmount"),
                        (L("EmployeeLevel")) + L("Name")
                            );

                        AddObjects(
                            sheet, categorySalary,
                        d => d.Salary,
                        d => d.Category,
                        d => d.TechnicalAmount,
                        d => d.EmployeeLevelName
                            );

                    });

        }
    }
}