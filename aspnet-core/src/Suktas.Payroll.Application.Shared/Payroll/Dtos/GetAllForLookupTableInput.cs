using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}