namespace TaxRuleTest.Models
{
     public partial class Employee
     {
          public string? BankAccountNo { get; set; }
          public int RowNo { get; set; }
          public int EmployeeContractId { get; set; }
          public int? ContractTypeId { get; set; }
          public string? Department { get; set; }
          public decimal SSFDeduction { get; set; }
          public decimal EmployeePVFDeduction { get; set; }
          public decimal EmployerPVFDeduction { get; set; }
          public decimal PersonalAllowance { get; set; }
          public decimal PersonalExpenses { get; set; }

          public string? StreetAddress { get; set; }
          public string? StreetAddressThai { get; set; }
          public string? SubDistrict { get; set; }
          public string? DistrictThai { get; set; }
          public string? SubDistrictThai { get; set; }
          public string? District { get; set; }
          public string? Province { get; set; }
          public string? ProvinceThai { get; set; }
          public string? PostalCode { get; set; }
          public string? CountryCode { get; set; }

          // Save temple value of ReportingToEmployeeId Field
          public string SupervisorId { get; set; }
          public List<EmployeeContract> EmployeeContracts { get; set; } = new();

          public EmployeeContract LatestContract
          {
               get
               {
                    return EmployeeContracts.OrderByDescending(_ => _.EmployeeContractId).FirstOrDefault();
               }
          }

          public string ContractType
          {
               get
               {
                    if (LatestContract != null && LatestContract.ContractType != null)
                    {
                         return LatestContract.ContractType.ContractName;
                    }
                    return "";
               }
          }

          public string DepartmentName
          {
               get
               {
                    if (LatestContract != null && LatestContract.ClientUnit != null)
                    {
                         return LatestContract.ClientUnit.Name;
                    }
                    return "";
               }
          }

          public string Position
          {
               get
               {
                    if (LatestContract != null && LatestContract.Position != null)
                    {
                         return LatestContract.Position.PositionName;
                    }
                    return "";
               }
          }

          public string Grade
          {
               get
               {
                    if (LatestContract != null && LatestContract.Grade != null)
                    {
                         return LatestContract.Grade.GradeName;
                    }
                    return "";
               }
          }

          public decimal? PayRateOnshore
          {
               get
               {
                    if (LatestContract != null)
                    {
                         return LatestContract.PayRateOnshore;
                    }
                    return null;
               }
          }

          public decimal? PayRatePerHour
          {
               get
               {
                    if (LatestContract != null)
                    {
                         return LatestContract.PayRateOvertimePerHr;
                    }
                    return null;
               }
          }

          public string PaymentFrequency
          {
               get
               {
                    if (LatestContract != null && LatestContract.RateFrequency != null)
                    {
                         return LatestContract.RateFrequency.Name;
                    }
                    return "";
               }
          }

          public DateTime? EndDate
          {
               get
               {
                    if (LatestContract != null)
                    {
                         return LatestContract.EndDate;
                    }
                    return null;
               }
          }

          public string EndReason
          {
               get
               {
                    if (LatestContract != null && LatestContract.EmployeeContractEndReason != null)
                         return LatestContract.EmployeeContractEndReason.Name;
                    return "";
               }
          }
     }
}