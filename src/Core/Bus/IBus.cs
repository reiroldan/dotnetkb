using System;
using DotNetKillboard.Commands;
using DotNetKillboard.Events;

namespace DotNetKillboard.Bus
{

    /// <summary>
    /// A bus which handles sending commands and publishing events
    /// </summary>
    public interface IBus
    {
        /// <summary>
        /// Send a command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        void Send<T>(T command) where T : ICommand;

        /// <summary>
        /// Publish an event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        void Publish<T>(T @event) where T : IEvent;

        /// <summary>
        /// Register the specified command type and handler
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandHandlerType"></param>
        void RegisterCommand(Type commandType, Type commandHandlerType);

        /// <summary>
        /// Register the specified event type and handler
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventHandlerType"></param>
        void RegisterEvent(Type eventType, Type eventHandlerType);
    }

    public static class IBusExtensions
    {
        /// <summary>
        /// Register the specified command type and handler
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TCommandHandler"></typeparam>
        /// <param name="bus"></param>
        public static void RegisterCommand<TCommand, TCommandHandler>(this IBus bus)
            where TCommand : ICommand
            where TCommandHandler : ICommandHandler<TCommand> {
            bus.RegisterCommand(typeof(TCommand), typeof(TCommandHandler));
        }

        /// <summary>
        /// Register the specified event type and handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <param name="bus"></param>
        public static void RegisterEvent<TEvent, TEventHandler>(this IBus bus)
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent> {
            bus.RegisterEvent(typeof(TEvent), typeof(TEventHandler));
        }

    }
}