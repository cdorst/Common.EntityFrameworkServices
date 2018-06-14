using Microsoft.Extensions.Logging;
using System;

namespace Common.EntityFrameworkServices
{
    public abstract class MappingService<TInput, TOutput> : IMappingService<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        protected readonly ILogger<MappingService<TInput, TOutput>> _logger;

        public MappingService(ILogger<MappingService<TInput, TOutput>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected virtual Func<TInput, int, TOutput> Result { get; set; } = (input, parentId) => default;

        public TOutput Map(TInput input, int parentId = 0)
        {
            _logger.LogInformation("Returning mapped object");
            return Result(input, parentId);
        }
    }
}
