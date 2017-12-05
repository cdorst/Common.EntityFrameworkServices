using System.Collections.Generic;

namespace Common.EntityFrameworkServices
{
    public interface IParentMappingService<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        TOutput Map(List<TInput> input);
    }
}
