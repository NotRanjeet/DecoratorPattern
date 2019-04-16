using Core.Dtos;
using Core.Interfaces;
using Logic.Decorators;
using Logic.Entities;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.AppServices
{
    public class GetTodoListQuery : IQuery<List<TodoDto>>
    {
        /// <summary>
        /// If true get the completed items only
        /// Otherwise fetch all of them without status filter
        /// </summary>
        public bool CompletedOnly { get; set; }

        public GetTodoListQuery(bool completedOnly = false)
        {
            CompletedOnly = completedOnly;
        }
        internal sealed class GetTodoListQueryHandler : IQueryHandler<GetTodoListQuery, List<TodoDto>>
        {
            private readonly IRepository repository;

            public GetTodoListQueryHandler(IRepository repository)
            {
                this.repository = repository;
            }

            public List<TodoDto> Handle(GetTodoListQuery query)
            {
                var todos = repository.List<TodoItem>();
                //Get completed only If user sepcifically asked For
                if (query.CompletedOnly)
                {
                    todos.Where(i=>i.IsDone);
                }
                return todos.Select(i => new TodoDto
                {
                    Title = i.Title,
                    Description = i.Description,
                    IsDone = i.IsDone,
                }).ToList();
            }
        }
    }
}
