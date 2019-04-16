using Core.Interfaces;
using CSharpFunctionalExtensions;
using Logic.Entities;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.AppServices
{
    public sealed class ToggleItemCommand: ICommand
    {
        private int Id {get;}

        private bool Done {get;set;}

        public ToggleItemCommand(int id, bool status)
        {
            Id = id;
            Done = status;
        }

        public sealed class ToggleItemCommandHandler: ICommandHandler<ToggleItemCommand>
        {
            private readonly IRepository repository;

            public ToggleItemCommandHandler(IRepository repository)
            {
                this.repository = repository;
            }

            public Result Handle(ToggleItemCommand command)
            {
                var item = repository.GetById<TodoItem>(command.Id);
                if(item == null)
                {
                    return Result.Fail($"No Item found with Id: {command.Id}");
                }
                item.IsDone = command.Done;
                if (command.Done)
                {
                    item.CompleteDate = DateTime.UtcNow;
                }
                repository.Update<TodoItem>(item);
                return Result.Ok();
            }
        }
    }
}
