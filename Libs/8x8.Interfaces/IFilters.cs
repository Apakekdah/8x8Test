namespace _8x8.Interfaces
{
    public interface IFilterRule
    {

    }

    public interface IFilterRule<TFilter1, TFilter2, TFilter3, TFilter4> : IFilterRule
        where TFilter1 : notnull
        where TFilter2 : notnull
        where TFilter3 : notnull
        where TFilter4 : notnull
    {
        TFilter1 Filter1 { get; set; }
        TFilter2 Filter2 { get; set; }
        TFilter3 Filter3 { get; set; }
        TFilter4 Filter4 { get; set; }
    }

    public interface IFilterRule<TFilter1, TFilter2, TFilter3> : IFilterRule
        where TFilter1 : notnull
        where TFilter2 : notnull
        where TFilter3 : notnull
    {
        TFilter1 Filter1 { get; set; }
        TFilter2 Filter2 { get; set; }
        TFilter3 Filter3 { get; set; }
    }
}