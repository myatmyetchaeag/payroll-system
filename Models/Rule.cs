namespace TaxRuleTest.Models
{
     public class Rule
    {
        public int RuleID { get; set; }
        public string? RuleName { get; set; }
        public string? RuleType { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public List<RuleCondition>? Conditions { get; set; }
        public List<RuleAction>? Actions { get; set; }
    }
}