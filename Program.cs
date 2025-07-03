using System;
using System.Collections.Generic;
using System.Linq;
using TaxRuleTest;
using TaxRuleTest.Models;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Dynamic Income Tax Calculator ===");

        var rules = TaxRuleCalculator.LoadRulesFromFile("TaxRules.json");

        // var employee = GetMockEmployee();
        // EmployeeContract contract = employee.EmployeeContracts
        //     .OrderByDescending(c => c.EmployeeContractId)
        //     .FirstOrDefault() ?? new EmployeeContract();
        // var context = MapEmployeeToTaxContext(employee, contract);
        // var (rate, tax, method) = TaxRuleCalculator.Evaluate(rules, context);

        var employee = GetMockEmployeeForPayroll();

        var result = PayrollCalculator.CalculateDetailUsingTaxCalculator(employee, rules, month: 7, year: 2025);

        
        Console.WriteLine($"\n=== Monthly Payroll Breakdown ===");
        Console.WriteLine($"Country               : {result.Country}");
        Console.WriteLine($"Salary                : {result.MonthlySalary:N2}");
        Console.WriteLine($"Overtime (10%)        : {result.Overtime10:N2}");
        Console.WriteLine($"Overtime (15%)        : {result.Overtime15:N2}");
        Console.WriteLine($"Overtime (20%)        : {result.Overtime20:N2}");
        Console.WriteLine($"Overtime (30%)        : {result.Overtime30:N2}");
        Console.WriteLine($"Total Earnings        : {result.TotalEarnings:N2}");
        Console.WriteLine($"Annual Earnings       : {result.AnnualEarnings:N2}");
        Console.WriteLine($"Allowances            : {result.Allowances:N2}");
        Console.WriteLine($"Deductions            : {result.Deductions:N2}");
        // Console.WriteLine($"Tax Method            : {result.TaxMethod}");
        Console.WriteLine($"Tax Rate              : {result.TaxRate:P}");
        Console.WriteLine($"Annual Tax            : {result.AnnualTax:N2}");
        Console.WriteLine($"Monthly Tax           : {result.MonthlyTax:N2}");
        Console.WriteLine($"Net Income            : {result.NetIncome:N2}");
        Console.WriteLine($"Annual Net Income     : {result.AnnualNetIncome:N2}");
        Console.WriteLine($"Grand Total           : {result.GrandTotal:N2}"); ;
    }

    static Employee GetMockEmployee()
    {
        Employee employee1 = new Employee
        {
            PersonalAllowance = 0,  // Has spouse?
            PersonalExpenses = 0,   // children?
            Country = "TH",
            EmployeeContracts = new List<EmployeeContract>
            {
                new EmployeeContract
                {
                    EmployeeContractId = 1,
                    PayRateOnshore = 100000, // Monthly salary
                    StartDate = new DateTime(2023, 1, 1),
                    ContractType = new ContractType { ContractName = "Full-Time" },
                    ClientUnit = new ClientUnit { Name = "Finance" },
                    Position = new Position { PositionName = "Software Engineer" },
                    Grade = new Grade { GradeName = "P2" },
                    RateFrequency = new RateFrequency { Name = "Monthly" },
                    EmployeeContractEndReason = new EmployeeContractEndReason { Name = "N/A" }
                }
            }
        };

        Employee employee2 =new Employee
        {
            Country = "SG",
            EmployeeContracts = new List<EmployeeContract>
            {
                new EmployeeContract
                {
                    EmployeeContractId = 2,
                    PayRateOnshore = 5000, // Monthly salary
                    StartDate = new DateTime(2024, 1, 1),
                    ContractType = new ContractType { ContractName = "Full-Time" },
                    ClientUnit = new ClientUnit { Name = "IT" },
                    Position = new Position { PositionName = "Software Engineer" },
                    Grade = new Grade { GradeName = "P2" },
                    RateFrequency = new RateFrequency { Name = "Monthly" },
                    EmployeeContractEndReason = new EmployeeContractEndReason { Name = "N/A" }
                }
            }
        };

        Employee employee3 =new Employee
        {
            Country = "MY",
            EmployeeContracts = new List<EmployeeContract>
            {
                new EmployeeContract
                {
                    EmployeeContractId = 3,
                    PayRateOnshore = 4000, // Monthly salary
                    StartDate = new DateTime(2024, 1, 1),
                    ContractType = new ContractType { ContractName = "Full-Time" },
                    ClientUnit = new ClientUnit { Name = "IT" },
                    Position = new Position { PositionName = "Software Engineer" },
                    Grade = new Grade { GradeName = "P2" },
                    RateFrequency = new RateFrequency { Name = "Monthly" },
                    EmployeeContractEndReason = new EmployeeContractEndReason { Name = "N/A" }
                }
            }
        };

        return employee1;
    }

    static Employee GetMockEmployeeForPayroll()
    { 
        return new Employee
        {
            PersonalAllowance = 0,  // Has spouse
            PersonalExpenses = 0,   // 2 children
            Country = "TH",
            EmployeeContracts = new List<EmployeeContract>
            {
                new EmployeeContract
                {
                    EmployeeContractId = 1,
                    EmployeeId = 1,
                    PayRateOnshore = 100000,          // Monthly salary: 100,000 THB
                    PayRateOvertimePerHr = 0,       // OT rate per hour
                    StartDate = new DateTime(2023, 1, 1),
                    ProjectedEndDate = new DateTime(2025, 12, 31),
                    CompanyCoversTax = false,
                    CompanyCoversSSF = false,
                    ContractType = new ContractType { ContractName = "Full-Time" },
                    ClientUnit = new ClientUnit { Name = "Engineering" },
                    Position = new Position { PositionName = "Senior Developer" },
                    Grade = new Grade { GradeName = "P3" },
                    RateFrequency = new RateFrequency { Name = "Monthly" },
                    EmployeeContractEndReason = new EmployeeContractEndReason { Name = "Active" }
                }
            }
        };
    }

    static TaxContext MapEmployeeToTaxContext(Employee employee, EmployeeContract contract)
    {
        return new TaxContext
        {
            AnnualIncome = contract.PayRateOnshore.GetValueOrDefault() * 12,
            HasSpouse = employee.PersonalAllowance >= 60000,
            NumChildren = (int)(employee.PersonalExpenses / 30000),
            Country = employee.Country
        };
    }

}
