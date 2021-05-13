using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.JsonQL.Sample.AspNetCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();
            services.AddDbContext<DataContext>(builder =>
            {
                builder.UseSqlite(@"Data Source=.\Database.sqlite");
            });

            //配置JsonQL的资源列表
            services.AddJsonQL(options =>
            {
                options.AuthorizeAsync = context => Task.FromResult(true);
                options.JsonQLIncluder = new DynamicJsonQLIncluder();
                options.JsonQLEvaluator = new DynamicJsonQLEvaluator();
                options.JsonQLLinqer = new DynamicJsonQLLinqer();
            }, serviceProvider =>
            {
                var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
                var resourceTable = new JsonQLResourceTable();
                resourceTable.AddResource("Orders", new Resource<Order>(context.Orders.AsQueryable()));
                resourceTable.AddResource("OrderStatusChanges", new Resource<OrderStatusChange>(context.OrderStatusChanges.AsQueryable()));
                resourceTable.AddResource("UserAccounts", new Resource<UserAccount>(context.UserAccounts.AsQueryable()));
                resourceTable.AddResource("UserAccountRecords", new Resource<UserAccountRecord>(context.UserAccountRecords.AsQueryable()));
                resourceTable.AddResource("Users", new Resource<User>(context.Users.AsQueryable()));
                resourceTable.AddResource("UserProfiles", new Resource<UserProfile>(context.UserProfiles.AsQueryable()));
                return resourceTable;
            });
            services.AddJsonQLTester(options =>
            {
                options.ResourceTypes = new Dictionary<string, Type>
                {
                    {"Orders",typeof(Order)},
                    {"OrderStatusChanges",typeof(OrderStatusChange)},
                    {"UserAccounts",typeof(UserAccount)},
                    {"UserAccountRecords",typeof(UserAccountRecord)},
                    {"Users",typeof(User)},
                    {"UserProfiles",typeof(UserProfile)},
                };
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapJsonQL();
                endpoints.MapJsonQLTester();
                endpoints.Map("/", async context =>
                {
                    context.Response.Redirect("/jsonQL-tester");
                    await Task.CompletedTask;
                });
            });
        }
    }
}
