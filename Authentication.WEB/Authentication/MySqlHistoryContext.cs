using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;

namespace InsuredTraveling
{
    public class MySqlHistoryContext : HistoryContext
    {
        public MySqlHistoryContext(
        DbConnection existingConnection,
        string defaultSchema)
        : base(existingConnection, defaultSchema)
            {
            }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(10).IsRequired();
            modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(20).IsRequired();
        }
    }
}