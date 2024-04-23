﻿using Suktas.Payroll.Editions.Dto;

namespace Suktas.Payroll.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < PayrollConsts.MinimumUpgradePaymentAmount;
        }
    }
}
