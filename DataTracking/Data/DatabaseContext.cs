using DataTracking.Models;
using DataTracking.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Text;

namespace DataTracking.Data
{
    public class DatabaseContext: DbContext
    {        
        private readonly GetUserContext _userContext;
        public DatabaseContext(
            DbContextOptions<DatabaseContext> options,
            GetUserContext userContext
          ) : base(options) 
        { _userContext = userContext; }
        public DbSet<Products> products { get; set; }
        public DbSet<AuditLog> auditLogs { get; set; }
        //override method savechanges
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changeEntity = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)
                .ToList();           
            foreach (var entity in changeEntity) {
                var logging = new AuditLog
                {
                    session_id = _userContext.getSessionValue(), //header
                    action = entity.State.ToString(),
                    entity = entity.Entity.GetType().Name,
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    data = $"{GetModified(entity)}",
                };
                auditLogs.Add(logging);
            }
            return base.SaveChangesAsync(cancellationToken);
        }     
        private string GetModified(EntityEntry entityEntry)
        {
            var changeData = new StringBuilder();
            foreach (var entity in entityEntry.OriginalValues.Properties)
            {
                var oldData = entityEntry.OriginalValues[entity];
                var newData = entityEntry.CurrentValues[entity];
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                        changeData.AppendLine($"{entity.Name}:{newData} ");
                        break;
                    case EntityState.Modified:
                        changeData.AppendLine($"{entity.Name}:{oldData}|{newData} ");
                        break;
                }               
            }
           
            return changeData.ToString();
        }

    }
}
