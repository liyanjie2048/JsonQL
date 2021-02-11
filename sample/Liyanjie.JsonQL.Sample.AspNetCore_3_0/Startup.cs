using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Liyanjie.JsonQL.Sample.AspNetCore_3_0
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
                var dataContext = services.BuildServiceProvider(false).GetService<DataContext>();
                options.ResourceTable
                    .AddResource("Orders", dataContext.Orders.AsQueryable())
                    .AddResource("OrderStatusChanges", dataContext.OrderStatusChanges.AsQueryable())
                    .AddResource("UserAccounts", dataContext.UserAccounts.AsQueryable())
                    .AddResource("UserAccountRecords", dataContext.UserAccountRecords.AsQueryable())
                    .AddResource("Users", dataContext.Users.AsQueryable())
                    .AddResource("UserProfiles", dataContext.UserProfiles.AsQueryable());
                options.AuthorizeAsync = context => Task.FromResult(true);
                options.JsonQLIncluder = new DynamicJsonQLIncluder();
                options.JsonQLEvaluator = new DynamicJsonQLEvaluator();
                options.JsonQLLinqer = new DynamicJsonQLLinqer();
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
