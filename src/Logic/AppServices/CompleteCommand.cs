using Core.Interfaces;
using CSharpFunctionalExtensions;
using Logic.Entities;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.AppServices
{
    public class CompleteCommand: ICommand
    {
        private int Id {get;}

        private bool Done {get;set;}

        public CompleteCommand(int id, bool status)
        {
            Id = id;
            Done = status;
        }

        internal sealed class CompleteCommandHandler: ICommandHandler<CompleteCommand>
        {
            private readonly IRepository repository;

            public CompleteCommandHandler(IRepository repository)
            {
                this.repository = repository;
            }

            public Result Handle(CompleteCommand command)
            {
                var item = repository.GetById<TodoItem>(command.Id);
                if(item == null)
                {
                    Result.Fail($"No Item found with Id: {command.Id}");
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
