using System;
using System.Collections.Generic;
using System.Threading;
using DotNetKillboard.Commands;
using DotNetKillboard.Events;

namespace DotNetKillboard.Bus
{

    /// <summary>
    /// Simple in memory bus implementation
    /// </summary>
    public class InMemoryBus : IBus
    {

        private readonly Type _commandType = typeof(ICommand);
        private readonly Type _commandHandlerType = typeof(ICommandHandler<>);
        private readonly Type _eventType = typeof(IEvent);
        private readonly Type _eventHandlerType = typeof(IEventHandler<>);

        private readonly IResolver _resolver;
        private readonly Dictionary<Type, List<Type>> _eventRoutes = new Dictionary<Type, List<Type>>();
        private readonly Dictionary<Type, Type> _commandRoutes = new Dictionary<Type, Type>();

        public InMemoryBus(IResolver resolver) {
            _resolver = resolver;
        }

        public void RegisterEvent(Type eventType, Type eventHandlerType) {
            if (!_eventType.IsAssignableFrom(eventType))
                throw new InvalidOperationException(string.Format("Cannot register type {0} as its not a event type.", eventType.Name));

            if (!eventHandlerType.IsAssignableToGenericType(_eventHandlerType))
                throw new InvalidOperationException(string.Format("Cannot register type {0} as its not a event handler type.", eventHandlerType.Name));

            List<Type> handlers;

            if (!_eventRoutes.TryGetValue(eventType, out handlers)) {
                handlers = new List<Type>();
                _eventRoutes.Add(eventType, handlers);
            }

            handlers.Add(eventHandlerType);
        }

        public void RegisterCommand(Type commandType, Type commandHandlerType) {
            if (!_commandType.IsAssignableFrom(commandType))
                throw new InvalidOperationException(string.Format("Cannot register type {0} as its not a command type.", commandType.Name));

            if (!commandHandlerType.IsAssignableToGenericType(_commandHandlerType))
                throw new InvalidOperationException(string.Format("Cannot register type {0} as its not a command handler type.", commandHandlerType.Name));

            if (_commandRoutes.ContainsKey(commandType))
                throw new InvalidOperationException(
                    string.Format("There is already a command handler registered for command type {0}.", commandType));

            _commandRoutes.Add(commandType, commandHandlerType);
        }

        public void Send<T>(T command) where T : ICommand {
            var commandType = typeof(T);

            Type handlerType;

            if (_commandRoutes.TryGetValue(commandType, out handlerType)) {
                var handler = (ICommandHandler<T>)_resolver.TryResolve(handlerType);

                if (handler != null) {
                    handler.Handle(command);
                    return;
                }
            }

            throw new InvalidOperationException(string.Format("Could not resolve command handler for {0}", command.GetType()));
        }

        public void Publish<T>(T @event) where T : IEvent {
            List<Type> handlers;

            var genEvtType = _eventHandlerType.MakeGenericType(@event.GetType());
            var genMethod = genEvtType.GetMethod("Handle");

            if (!_eventRoutes.TryGetValue(@event.GetType(), out handlers))
                return;

            foreach (var handlerType in handlers) {
                var handler = _resolver.TryResolve(handlerType);

                if (handler == null)
                    throw new Exception(string.Format("Could not resolve event handler of type {0}", handlerType.Name));

                Action action = () => genMethod.Invoke(handler, new object[] { @event });

                if (@event.Async)
                    ThreadPool.QueueUserWorkItem(x => action());
                else {
                    action();                    
                }
            }
        }
    }
}