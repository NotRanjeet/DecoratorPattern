using Logic.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem
                {
                    Id = 1,
                    Title = "Get Sample Working",
                    Description = "Try to get the sample to build.",
                    Created = DateTime.Now,
                    IsDone = false,
                    CompleteDate = null,
                },
                new TodoItem
                {
                    Id = 2,
                    Title = "Review Solution",
                    Description = "Review the different projects in the solution and how they relate to one another.",
                    Created = DateTime.Now,
                    IsDone = false,
                    CompleteDate = null
                },
                new TodoItem
                {
                    Id = 3,
                    Title = "Run and Review Tests",
                    Description = "Make sure all the tests run and review what they are doing.",
                    Created = DateTime.Now,
                    IsDone = false,
                    CompleteDate = null
                }
            );
            
        }
    }
}
