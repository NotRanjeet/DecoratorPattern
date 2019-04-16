using System;
using CSharpFunctionalExtensions;
using Logic.Interfaces;
using SimpleInjector;

namespace Logic.Todo
{
    public sealed class Messages
    {
        private readonly Container _provider;

        public Messages(Container container)
        {
            _provider = container;
        }

        public Result Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetInstance(handlerType);
            Result result = handler.Handle((dynamic)command);

            return result;
        }

        public T Dispatch<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetInstance(handlerType);
            T result = handler.Handle((dynamic)query);

            return result;
        }
    }
}
