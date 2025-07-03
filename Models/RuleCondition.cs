namespace TaxRuleTest.Models
{
     public class RuleCondition
     {
          public string? Parameter { get; set; }
          public string? Operator { get; set; }
          public object? Value { get; set; }
          public object? GroupLogic { get; set; }
     }
}