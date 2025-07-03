using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using TaxRuleTest.Models;

namespace TaxRuleTest
{
    public static class PayrollCalculator
    {
        public static PayrollResult CalculateAnnualPayroll(Employee employee, List<Timesheet> timesheets, List<Rule> rules)
        {
            var countryCode = employee.CountryCode ?? "TH";

            var contract = employee.LatestContract;
            if (contract == null) throw new InvalidOperationException("No active contract found.");

            var startDate = contract.StartDate;


            return new PayrollResult
            {
                CountryCode = countryCode,

            };
        }
    }
}
