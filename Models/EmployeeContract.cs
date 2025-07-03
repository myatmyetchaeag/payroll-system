using System;

namespace TaxRuleTest.Models
{
    public class EmployeeContract
    {
        public int EmployeeContractId { get; set; }
        public int EmployeeId { get; set; }
        public int? ContractTypeId { get; set; }
        public int? ClientUnitId { get; set; }
        public int? PositionID { get; set; }
        public int? GradeID { get; set; }
        public int? RateFrequencyId { get; set; }
        public int? EmployeeContractEndReasonId { get; set; }

        public string? JobTitle { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ProbabationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ProjectedEndDate { get; set; }

        public decimal? PayRateOnshore { get; set; }
        public decimal? PayRateOvertimePerHr { get; set; }
        public bool? CompanyCoversSSF { get; set; }
        public bool? CompanyCoversTax { get; set; }

        public decimal? TotalIncomePrevious { get; set; }
        public decimal? TotalTaxPrevious { get; set; }

        // Navigation properties
        public ContractType? ContractType { get; set; }
        public ClientUnit? ClientUnit { get; set; }
        public Position? Position { get; set; }
        public Grade? Grade { get; set; }
        public RateFrequency? RateFrequency { get; set; }
        public EmployeeContractEndReason? EmployeeContractEndReason { get; set; }
    }

    public class ContractType
    {
        public string? ContractName { get; set; }
    }

    public class ClientUnit
    {
        public string? Name { get; set; }
    }

    public class Position
    {
        public string? PositionName { get; set; }
    }

    public class Grade
    {
        public string? GradeName { get; set; }
    }

    public class RateFrequency
    {
        public string? Name { get; set; }
    }

    public class EmployeeContractEndReason
    {
        public string? Name { get; set; }
    }
}
