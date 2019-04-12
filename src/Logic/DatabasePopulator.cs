using Core.Interfaces;
using Logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public static class DatabasePopulator
    {
        public static int PopulateDatabase(IRepository todoRepository)
        {
            if (todoRepository.List<TodoItem>().Any()) return 0;

            todoRepository.Add(new TodoItem
            {
                Title = "Get Sample Working",
                Description = "Try to get the sample to build."
            });
            todoRepository.Add(new TodoItem
            {
                Title = "Review Solution",
                Description = "Review the different projects in the solution and how they relate to one another."
            });
            todoRepository.Add(new TodoItem
            {
                Title = "Run and Review Tests",
                Description = "Make sure all the tests run and review what they are doing."
            });

            return todoRepository.List<TodoItem>().Count;
        }
    }
}
