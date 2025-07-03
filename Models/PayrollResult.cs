namespace TaxRuleTest.Models
{
     public class PayrollResult
     {
          public string CountryCode { get; set; }

          public decimal AnnualGrossIncome { get; set; }
          public decimal TotalAllowances { get; set; }
          public decimal TotalDeductions { get; set; }
          public decimal TaxableIncome { get; set; }
          public decimal AnnualTax { get; set; }
          public decimal AnnualNetIncome { get; set; }

     }
}
