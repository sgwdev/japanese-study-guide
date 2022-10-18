using Core.Entities;
using Core.Entities.KanjiAggregate;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace IntegrationTests.Data
{
    public abstract class BaseRepositoryTestFixture
    {
        protected AppDbContext _appDbContext;

        protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("japanesestudyguide")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        protected IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            var options = CreateNewContextOptions();
            _appDbContext = new AppDbContext(options);
            _appDbContext.Database.EnsureCreated();

            return new Repository<T>(_appDbContext);
        }        
    }
}
