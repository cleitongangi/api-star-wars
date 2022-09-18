namespace StarWars.Infra.IoC.RestAPI
{
    public static class MigrationManager
    {
        public static void ApplyMigration(IServiceProvider serviceProvider)
        {
            //using var scope = serviceProvider.CreateScope();
            //using var appContext = scope.ServiceProvider.GetRequiredService<ISoccerManagerDbContext>();
            //appContext.Database.Migrate();
        }
    }
}
