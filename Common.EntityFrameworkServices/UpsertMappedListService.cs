using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices.Services
{
    public class UpsertMappedListService<TDbContext, TInput, TOutput> : IUpsertMappedListService<TDbContext, TInput, TOutput>
        where TDbContext : DbContext
        where TInput : class
        where TOutput : class
    {
        private readonly ILogger<UpsertMappedListService<TDbContext, TInput, TOutput>> _logger;
        private readonly IMappingService<TInput, TOutput> _mappingService;
        private readonly IUpsertListService<TDbContext, TOutput> _upsertListService;

        public UpsertMappedListService(
            ILogger<UpsertMappedListService<TDbContext, TInput, TOutput>> logger,
            IMappingService<TInput, TOutput> mappingService,
            IUpsertListService<TDbContext, TOutput> upsertListService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
            _upsertListService = upsertListService ?? throw new ArgumentNullException(nameof(upsertListService));
        }

        public async Task<List<TOutput>> UpsertAsync(List<TInput> list, int parentId = 0)
        {
            _logger.LogInformation("Mapping list and upserting records");
            return await _upsertListService.UpsertAsync(
                list.Select(each => _mappingService.Map(each, parentId)).ToList());
        }
    }
}
