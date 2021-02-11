using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Liyanjie.JsonQL.Sample.AspNetCore_3_0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            MigrateDbContext(host);
            InitializeDbContext(host);
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        static void MigrateDbContext(IWebHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<DataContext>>();
            var context = services.GetRequiredService<DataContext>();

            try
            {
                logger.LogInformation($"Migrating database associated with context {nameof(DataContext)}");

                context.Database.Migrate();

                logger.LogInformation($"Migrated database associated with context {nameof(DataContext)}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while migrating the database used on context {nameof(DataContext)}");
            }
        }

        static void InitializeDbContext(IWebHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<DataContext>>();
            var context = services.GetRequiredService<DataContext>();

            try
            {
                logger.LogInformation($"Initialize data with context {nameof(DataContext)}");

                #region Data
                if (context.Users.AsNoTracking().Count() == 0)
                {
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
                }
                #endregion

                context.SaveChanges();

                logger.LogInformation($"Initialized data with context {nameof(DataContext)}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while initialized data used on context {nameof(DataContext)}");
            }
        }
    }
}
