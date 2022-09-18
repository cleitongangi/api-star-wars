using StarWars.Domain.Interfaces.Data;

namespace StarWars.Infra.Data.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StarWarsDbContext _db;

        public UnitOfWork(StarWarsDbContext starWarsDbContext)
        {
            this._db = starWarsDbContext;
        }

        public async Task BeginTransactionAsync()
        {
            if (_db.Database.CurrentTransaction == null)
                await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_db.Database.CurrentTransaction != null)
                await _db.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            if (_db.Database.CurrentTransaction != null)
                await _db.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
