using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Liyanjie.JsonQL.Sample.AspNet
{
    public class Global : HttpApplication
    {
        static bool uninitialized = true;
        static DataContext dataContext;

        protected void Application_Start(object sender, EventArgs e)
        {
            if (uninitialized)
            {
                dataContext = new DataContext(ConfigurationManager.ConnectionStrings["SqlServerCe"].ConnectionString);
                IReadOnlyDictionary<string, Type> resourceTypes = null;
                this.AddJsonQL(options =>
                {
                    options.AuthorizeAsync = context => Task.FromResult(true);
                    //options.JsonQLIncluder = new DynamicJsonQLIncluder();
                    options.JsonQLEvaluator = new DynamicJsonQLEvaluator();
                    options.JsonQLLinqer = new DynamicJsonQLLinqer();
                }, resources =>
                {
                    resources
                        .AddResource("Orders", new Resource<Order>(dataContext.Orders.AsQueryable()))
                        .AddResource("OrderStatusChanges", new Resource<OrderStatusChange>(dataContext.OrderStatusChanges.AsQueryable()))
                        .AddResource("UserAccounts", new Resource<UserAccount>(dataContext.UserAccounts.AsQueryable()))
                        .AddResource("UserAccountRecords", new Resource<UserAccountRecord>(dataContext.UserAccountRecords.AsQueryable()))
                        .AddResource("Users", new Resource<User>(dataContext.Users.AsQueryable()))
                        .AddResource("UserProfiles", new Resource<UserProfile>(dataContext.UserProfiles.AsQueryable()));
                    resourceTypes = resources.ResourceTypes;
                });
                this.AddJsonQLTester(options =>
                {
                    options.ResourceTypes = resourceTypes?.ToDictionary(_ => _.Key, _ => _.Value);
                });
                uninitialized = false;
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            this.UseJsonQL();
            this.UseJsonQLTester();

            if ("/".Equals(Request.Path))
                Response.Redirect("~/jsonQL-tester");
        }
    }
}