using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TaxRuleTest.Models;

public static class TaxRuleCalculator
{
    public static List<Rule> LoadRulesFromFile(string path)
    {
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<List<Rule>>(json);
    }

    public static (decimal rate, decimal tax) Evaluate(List<Rule> allRules, TaxContext context)
    {
        var contextDict = context.ToDictionary();
        var country = context.Country;

        // Filter rules by country
        var rules = allRules
            .Where(r => r.IsActive && r.Conditions.Any(c =>
                c.Parameter == "Country" &&
                c.Value.ToString().Equals(country, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        // Apply Allowances
        context.CalculatedAllowances = ApplyAllowanceRules(rules, contextDict);

        // Apply Deductions
        context.CalculatedDeductions = ApplyDeductionRules(rules, contextDict);

        // Calculate Taxable Income
        var taxableIncome = context.GetTaxableIncome();
        contextDict["TaxableIncome"] = taxableIncome;

        // Progressive Tax (summed by bracket)
        var progressiveTax = ApplyProgressiveTax(rules, taxableIncome, contextDict, out decimal effectiveRate);

        // Flat Tax (only if applicable)
        var flatRule = rules
            .Where(r => r.RuleType == "Flat")
            .OrderBy(r => r.Priority)
            .FirstOrDefault(r => EvaluateConditions(r.Conditions, contextDict));

        decimal flatTax = 0, flatRate = 0;
        if (flatRule != null)
        {
            flatRate = flatRule.Actions.FirstOrDefault(a => a.ActionType == "ApplyFlatTaxRate")?.ActionValue ?? 0;
            flatTax = Math.Round(taxableIncome * flatRate, 2);
        }

        // Return only Progressive tax as requested
        return (effectiveRate, progressiveTax);
    }

    private static decimal ApplyAllowanceRules(List<Rule> rules, Dictionary<string, object> context)
    {
        decimal total = 0;

        foreach (var rule in rules.Where(r => r.RuleType == "Allowance"))
        {
            if (!EvaluateConditions(rule.Conditions, context)) continue;

            foreach (var action in rule.Actions)
            {
                if (action.ActionType == "ApplyFixedAllowance")
                    total += action.ActionValue;

                if (action.ActionType == "ApplyPerUnitAllowance" && context.TryGetValue("NumChildren", out var count))
                    total += action.ActionValue * Convert.ToInt32(count);
            }
        }

        return total;
    }

    private static decimal ApplyDeductionRules(List<Rule> rules, Dictionary<string, object> context)
    {
        decimal income = Convert.ToDecimal(context["AnnualIncome"]);
        decimal total = 0;

        foreach (var rule in rules.Where(r => r.RuleType == "Deduction"))
        {
            if (!EvaluateConditions(rule.Conditions, context)) continue;

            foreach (var action in rule.Actions)
            {
                if (action.ActionType == "ApplyStandardDeduction")
                {
                    var val = income * action.ActionValue;
                    if (rule.RuleName.Contains("Standard", StringComparison.OrdinalIgnoreCase) && val > 100000)
                        val = 100000;
                    total += val;
                }
                else if (action.ActionType == "ApplyFixedDeduction")
                {
                    total += action.ActionValue;
                }
            }
        }

        return total;
    }

    private static decimal ApplyProgressiveTax(List<Rule> rules, decimal taxableIncome, Dictionary<string, object> context, out decimal effectiveRate)
    {
        decimal totalTax = 0;
        effectiveRate = 0;

        var country = context["Country"].ToString();

        // Get all tax brackets for this country (ignore TaxableIncome filter in EvaluateConditions)
        var taxBrackets = rules
            .Where(r => r.RuleType == "Tax" &&
                        r.Conditions.Any(c =>
                            c.Parameter == "Country" &&
                            c.Value.ToString().Equals(country, StringComparison.OrdinalIgnoreCase)))
            .OrderBy(r => r.Priority)
            .ToList();

        foreach (var rule in taxBrackets)
        {
            var min = rule.Conditions
                .Where(c => c.Parameter == "TaxableIncome" && (c.Operator == ">" || c.Operator == ">="))
                .Select(c => Convert.ToDecimal(c.Value))
                .DefaultIfEmpty(0)
                .Max();

            var max = rule.Conditions
                .Where(c => c.Parameter == "TaxableIncome" && (c.Operator == "<" || c.Operator == "<="))
                .Select(c => Convert.ToDecimal(c.Value))
                .DefaultIfEmpty(decimal.MaxValue)
                .Min();

            if (taxableIncome <= min)
                continue;

            decimal incomeInBracket = Math.Min(taxableIncome, max) - min;
            if (incomeInBracket <= 0) continue;

            var rate = rule.Actions.FirstOrDefault(a => a.ActionType == "ApplyTaxRate")?.ActionValue ?? 0;
            totalTax += incomeInBracket * rate;
        }

        effectiveRate = taxableIncome > 0 ? totalTax / taxableIncome : 0;
        return Math.Round(totalTax, 2);
    }

    private static bool EvaluateConditions(List<RuleCondition> conditions, Dictionary<string, object> context)
    {
        foreach (var cond in conditions)
        {
            if (!context.TryGetValue(cond.Parameter, out var value))
                return false;

            if (cond.Value is string str && value is string actual)
            {
                if (!actual.Equals(str, StringComparison.OrdinalIgnoreCase))
                    return false;
                continue;
            }

            try
            {
                var left = Convert.ToDecimal(value);
                var right = Convert.ToDecimal(cond.Value);
                switch (cond.Operator)
                {
                    case ">": if (!(left > right)) return false; break;
                    case ">=": if (!(left >= right)) return false; break;
                    case "<": if (!(left < right)) return false; break;
                    case "<=": if (!(left <= right)) return false; break;
                    case "==": if (!(left == right)) return false; break;
                    case "!=": if (!(left != right)) return false; break;
                }
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    public static decimal getFixedDeductions(List<Rule> rules, Dictionary<string, object> context)
    {
        return rules
            .Where(r => r.RuleType == "Deduction" && r.IsActive && EvaluateConditions(r.Conditions, context))
            .SelectMany(r => r.Actions)
            .Where(a => a.ActionType == "ApplyFixedDeduction")
            .Sum(a => a.ActionValue);
    }
}
