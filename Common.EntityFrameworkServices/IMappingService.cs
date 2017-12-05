namespace Common.EntityFrameworkServices
{
    public interface IMappingService<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        TOutput Map(TInput input, int parentId = 0);
    }
}
