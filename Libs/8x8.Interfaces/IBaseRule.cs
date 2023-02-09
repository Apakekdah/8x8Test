namespace _8x8.Interfaces
{
    public interface IBaseRule : IStrategy
    {
        int RuleId { get; set; }
        int Priority { get; set; }
        int? OutputValue { get; set; }
    }
}