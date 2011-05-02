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
        /// Registers the specified <see cref="IMessage"/> to be handled by a specific action handler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        void RegisterHandler<T>(Action<T> handler) where T : IMessage;
    }
}