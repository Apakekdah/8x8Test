namespace _8x8.Interfaces
{
    public interface IFileReader<TResult>
        where TResult : class
    {
        TResult Reader(string path);
    }
}