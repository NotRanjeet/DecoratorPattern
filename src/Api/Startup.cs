using Api.Utils;
using Core.Interfaces;
using Infrastructure.Data;
using Logic.Todo;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            services.AddHandlers();

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }

        
    }
}
