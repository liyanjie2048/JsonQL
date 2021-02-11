using System.Data.Entity;

namespace Liyanjie.JsonQL.Sample.AspNet
{
    public class DataContext : DbContext
    {
        public DataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer(new DbInitializer());
        }

        public IDbSet<Order> Orders { get; set; }

        public IDbSet<OrderStatusChange> OrderStatusChanges { get; set; }

        public IDbSet<UserAccount> UserAccounts { get; set; }

        public IDbSet<UserAccountRecord> UserAccountRecords { get; set; }

        public IDbSet<User> Users { get; set; }

        public IDbSet<UserProfile> UserProfiles { get; set; }
    }
}
