using System.Collections.Generic;
using System.Data.Entity;

namespace Liyanjie.JsonQL.Sample.AspNet
{
    public class DbInitializer : CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            base.Seed(context);

            #region Data
            context.Users.Add(new User
            {
                Account = new UserAccount
                {
                    Coins = 7777.7M,
                    Points = 7777,
                },
                AccountRecords = new List<UserAccountRecord>
                {
                    new UserAccountRecord
                    {
                        Coins = 7777M,
                        Points = 7777,
                        Remark = "充值"
                    },
                    new UserAccountRecord
                    {
                        Coins = 0.7M,
                        Points = 0,
                        Remark = "奖励"
                    },
                },
                Orders = new List<Order>
                {
                    new Order
                    {
                        Serial = "001",
                        Status = 0,
                        StatusChanges = new List<OrderStatusChange>
                        {
                            new OrderStatusChange { Status = 0, },
                        }
                    },
                },
                Profile = new UserProfile
                {
                    Avatar = "Avatar",
                    Nick = "七月七",
                },
                Username = "u7777",
            });
            context.Users.Add(new User
            {
                Account = new UserAccount
                {
                    Coins = 8888.8M,
                    Points = 8888,
                },
                AccountRecords = new List<UserAccountRecord>
                {
                    new UserAccountRecord
                    {
                        Coins = 8888M,
                        Points = 8888,
                        Remark = "充值"
                    },
                    new UserAccountRecord
                    {
                        Coins = 0.8M,
                        Points = 0,
                        Remark = "奖励"
                    },
                },
                Orders = new List<Order>
                {
                    new Order
                    {
                        Serial = "002",
                        Status = 0,
                        StatusChanges = new List<OrderStatusChange>
                        {
                            new OrderStatusChange { Status = 0, },
                        }
                    },
                    new Order
                    {
                        Serial = "003",
                        Status = 2,
                        StatusChanges = new List<OrderStatusChange>
                        {
                            new OrderStatusChange { Status = 0, },
                            new OrderStatusChange { Status = 1, },
                            new OrderStatusChange { Status = 2, },
                        }
                    },
                },
                Profile = new UserProfile
                {
                    Avatar = "Avatar",
                    Nick = "八月八",
                },
                Username = "u8888",
            });
            context.Users.Add(new User
            {
                Account = new UserAccount
                {
                    Coins = 9999.9M,
                    Points = 9999,
                },
                AccountRecords = new List<UserAccountRecord>
                {
                    new UserAccountRecord
                    {
                        Coins = 9999M,
                        Points = 9999,
                        Remark = "充值"
                    },
                    new UserAccountRecord
                    {
                        Coins = 0.9M,
                        Points = 0,
                        Remark = "奖励"
                    },
                },
                Orders = new List<Order>
                {
                    new Order
                    {
                        Serial = "004",
                        Status = 1,
                        StatusChanges = new List<OrderStatusChange>
                        {
                            new OrderStatusChange { Status = 0, },
                            new OrderStatusChange { Status = 1, },
                        }
                    },
                    new Order
                    {
                        Serial = "005",
                        Status = 0,
                        StatusChanges = new List<OrderStatusChange>
                        {
                            new OrderStatusChange { Status = 0, },
                        }
                    },
                },
                Profile = new UserProfile
                {
                    Avatar = "Avatar",
                    Nick = "九月八",
                },
                Username = "u9999",
            });
            #endregion

            context.SaveChanges();
        }
    }
}