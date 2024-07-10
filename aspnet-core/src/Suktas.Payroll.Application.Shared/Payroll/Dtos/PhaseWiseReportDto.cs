namespace Suktas.Payroll.Payroll.Dtos
{
    public class PhaseWiseReportDto
    {
        public string Months { get; set; }
        public string Schools { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DueAmount { get; set; }
        public decimal ExtraAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string Remarks { get; set; }
    }
}
