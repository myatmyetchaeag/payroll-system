namespace TaxRuleTest.Models
{
    public class TaxContext
    {
        public decimal AnnualIncome { get; set; }
        public bool HasSpouse { get; set; }
        public int NumChildren { get; set; }
        public string? Country { get; set; }

        public decimal CalculatedAllowances { get; set; }
        public decimal CalculatedDeductions { get; set; }

        public decimal GetTaxableIncome()
        {
            return Math.Max(0, AnnualIncome - CalculatedAllowances - CalculatedDeductions);
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "AnnualIncome", AnnualIncome },
                { "HasSpouse", HasSpouse },
                { "NumChildren", NumChildren },
                { "Country", Country }
            };
        }
    }
}
