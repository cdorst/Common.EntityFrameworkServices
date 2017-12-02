namespace Common.EntityFrameworkServices.Services
{
    public interface IMappingService<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        TOutput Map(TInput input, int parentId = 0);
    }
}
