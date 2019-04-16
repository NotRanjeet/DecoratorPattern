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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using static Core.AppServices.GetTodoListQuery;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

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
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(dbName));
            services.AddTransient<IRepository, EfRepository>();
            services.AddSingleton<Messages>();
            //services.AddTransient<ICommandHandler<ToggleItemCommand>>(provider => 
            //    new AuditLoggingDecorator<ToggleItemCommand>(
            //        new ToggleItemCommand.ToggleItemCommandHandler(
            //            provider.GetService<IRepository>()
            //            )
            //        )
            //  );
            services.AddTransient<IQueryHandler<GetTodoListQuery, List<TodoDto>>, GetTodoListQueryHandler>();
            services.AddTransient<ICommandHandler<ToggleItemCommand>>(GenerateToggleItemCommandFactory());

        }

        private static Func<IServiceProvider, ICommandHandler<ToggleItemCommand>> GenerateToggleItemCommandFactory()
        {
            Func<IServiceProvider, ICommandHandler<ToggleItemCommand>> factory = (IServiceProvider provider) =>
            {
                //Get repository from DI Container
                var repository = provider.GetService<IRepository>();
                //Create command handler using the Repository
                var handler = new ToggleItemCommand.ToggleItemCommandHandler(repository);
                var decorated = new AuditLoggingDecorator<ToggleItemCommand>(handler);
                return decorated;

            };
            return factory;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }


    }
}
