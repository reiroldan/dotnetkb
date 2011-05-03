using System;
using System.Collections.Generic;
using System.Threading;
using DotNetKillboard.Commands;
using DotNetKillboard.Events;
using DotNetKillboard.Utils;

namespace DotNetKillboard.Bus
{

    /// <summary>
    /// Simple in memory bus implementation
    /// </summary>
    public class InMemoryBus : IBus
    {
        private readonly IResolver _resolver;
        private readonly Dictionary<Type, List<Action<IMessage>>> _routes = new Dictionary<Type, List<Action<IMessage>>>();
        private readonly Dictionary<Type, List<Type>> _eventRoutes = new Dictionary<Type, List<Type>>();
        private readonly Dictionary<Type, Type> _commandRoutes = new Dictionary<Type, Type>();

        public InMemoryBus(IResolver resolver) {
            _resolver = resolver;
        }

        public void RegisterEvent<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent> {

            List<Type> handlers;
            var eventType = typeof(TEvent);

            if (!_eventRoutes.TryGetValue(eventType, out handlers)) {
                handlers = new List<Type>();
                _eventRoutes.Add(eventType, handlers);
            }

            handlers.Add(typeof(TEventHandler));
        }

        public void RegisterCommand<TCommand, TCommandHandler>()
            where TCommand : ICommand
            where TCommandHandler : ICommandHandler<TCommand> {

            var commandType = typeof(TCommand);

            if (_commandRoutes.ContainsKey(commandType))
                throw new InvalidOperationException(
                    string.Format("There is already a handler registered for command type {0}.", commandType));

            _commandRoutes.Add(commandType, typeof(TCommandHandler));
        }

        public void Register<T>(Action<T> handler) where T : IMessage {
            List<Action<IMessage>> handlers;

            if (!_routes.TryGetValue(typeof(T), out handlers)) {
                handlers = new List<Action<IMessage>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add(DelegateAdjuster.CastArgument<IMessage, T>(x => handler(x)));
        }

        public void Send<T>(T command) where T : ICommand {
            List<Action<IMessage>> handlers;

            var commandType = typeof(T);

            if (_routes.TryGetValue(commandType, out handlers)) {
                if (handlers.Count != 1) {
                    throw new InvalidOperationException(string.Format("Found multiple handlers for {0}",
                                                                      commandType));
                }

                handlers[0](command);
                return;
            }

            Type handlerType;

            if (_commandRoutes.TryGetValue(commandType, out handlerType)) {
                var handler = (ICommandHandler<T>)_resolver.TryResolve(handlerType);

                if (handler != null) {
                    handler.Handle(command);
                    return;
                }
            }

            throw new InvalidOperationException(string.Format("no handler registered for {0}", command.GetType()));
        }

        public void Publish<T>(T @event) where T : IEvent {
            List<Action<IMessage>> handlers;

            if (!_routes.TryGetValue(@event.GetType(), out handlers))
                return;

            foreach (var handler in handlers) {
                var handler1 = handler;

                if (@event.Async)
                    ThreadPool.QueueUserWorkItem(x => handler1(@event));
                else {
                    handler1(@event);
                }
            }
        }
    }
}