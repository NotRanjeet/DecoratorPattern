using Api.Utils;
using Core.AppServices;
using Core.Dtos;
using Core.Interfaces;
using Infrastructure.Data;
using Logic.Decorators;
using Logic.Interfaces;
using Logic.Todo;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using static Core.AppServices.GetTodoListQuery;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        //Create only one instance of container for whole applications
        private Container container = new Container();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var config = new Config(3); // Deserialize from appsettings.json
            services.AddSingleton(config);
            string dbName = Guid.NewGuid().ToString();

            //Commented the old registrations for Dependency Injection
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseInMemoryDatabase(dbName));
            //services.AddTransient<IRepository, EfRepository>();
            //services.AddSingleton<Messages>();
            //services.AddHandlers();


            // Default lifestyle scoped + async
            // The recommendation is to use AsyncScopedLifestyle in for applications that solely consist of a Web API(or other asynchronous technologies such as ASP.NET Core)
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            //container register db context
            container.Register<AppDbContext>(() =>
            {
                var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(dbName).Options;
                return new AppDbContext(options);
            }, Lifestyle.Scoped);


            //Register all the services we are interested in
            container.RegisterInstance(config);
            container.Register<Messages>();
            container.Register<IRepository, EfRepository>(Lifestyle.Transient);


            //Register all command handlers
            container.Register(typeof(ICommandHandler<>), typeof(ICommandHandler<>).Assembly);
            //Register the CommandHandler with Decorators conditionally
            container.RegisterDecorator(typeof(ICommandHandler<>),
                typeof(AuditLoggingDecorator<>),
                context => context.ImplementationType.GetCustomAttributes(false).Any(a => a.GetType() == typeof(AuditLogAttribute)));

            //Register all query handlers
            container.Register(typeof(IQueryHandler<,>), typeof(IQueryHandler<,>).Assembly);


            // Register controllers DI resolution
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));

            // Wrap AspNet requests into Simpleinjector's scoped lifestyle
            services.UseSimpleInjectorAspNetRequestScoping(container);

            

        }





        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
            container.RegisterMvcControllers(app);
            container.Verify();
            using (AsyncScopedLifestyle.BeginScope(container))
            {
                //Make sure the Migrations are run
                var dbContext = container.GetInstance<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }


        }


    }
}
