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

        private readonly Dictionary<Type, List<Action<IMessage>>> _routes = new Dictionary<Type, List<Action<IMessage>>>();

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

            if (!_routes.TryGetValue(typeof(T), out handlers)) {
                throw new InvalidOperationException(string.Format("no handler registered for {0}", command.GetType()));
            }

            if (handlers.Count != 1) {
                var msg = string.Format("Found multiple handlers for {0}", command.GetType());
                throw new InvalidOperationException(msg);
            }

            handlers[0](command);
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