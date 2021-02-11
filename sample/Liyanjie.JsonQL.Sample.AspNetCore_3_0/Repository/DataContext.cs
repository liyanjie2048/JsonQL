using Microsoft.EntityFrameworkCore;

namespace Liyanjie.JsonQL.Sample.AspNetCore_3_0
{
    public class DataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderStatusChange> OrderStatusChanges { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<UserAccountRecord> UserAccountRecords { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
