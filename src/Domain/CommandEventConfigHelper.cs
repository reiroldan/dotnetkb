using System;
using System.Collections.Generic;
using System.Linq;
using DotNetKillboard.CommandHandlers;
using DotNetKillboard.Commands;
using DotNetKillboard.EventHandlers;
using DotNetKillboard.Events;

namespace DotNetKillboard
{
    public class CommandEventConfigHelper
    {
        private static readonly Type CommandType = typeof(ICommand);
        private static readonly Type CommandHandlerType = typeof(ICommandHandler<>);
        private static readonly Type EventType = typeof (IEvent);
        private static readonly Type EventHandlerType = typeof(IEventHandler<>);

        public static Dictionary<Type, Type> GetCommandAndHandlerTypes() {

            var commands = typeof(CreateAlliance).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && !x.IsInterface && CommandType.IsAssignableFrom(x));

            var commandHandlers = typeof(AllianceCommandHandlers).Assembly
                .GetTypes().Where(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableToGenericType(CommandHandlerType));

            var result = new Dictionary<Type, Type>();
            var errors = new List<string>();

            foreach (var command in commands) {
                var genCommandType = CommandHandlerType.MakeGenericType(command);
                var handlers = new List<Type>();

                foreach (var commandHandler in commandHandlers) {
                    var type = commandHandler.GetInterfaces().FirstOrDefault(genCommandType.IsAssignableFrom);

                    if (type != null)
                        handlers.Add(commandHandler);
                }
                
                if (handlers.Count != 1) {
                    errors.Add(string.Format("Found {0} handlers for command type {1}", handlers.Count,
                                                      command.Name));
                    continue;
                }

                result.Add(command, handlers.First());
            }

            if (errors.Count > 0) {
                throw new Exception(string.Join("\r\n", errors));
            }

            return result;
        }

        public static Dictionary<Type, List<Type>> GetEventAndHandlerTypes() {
            var events = typeof(AllianceCreated).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && !x.IsInterface && EventType.IsAssignableFrom(x));

            var eventHandlers = typeof(AllianceEventHandlers).Assembly
                .GetTypes().Where(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableToGenericType(EventHandlerType));

            var result = new Dictionary<Type, List<Type>>();
            var errors = new List<string>();

            foreach (var evnt in events) {
                var genEvntType = EventHandlerType.MakeGenericType(evnt);
                var handlers = new List<Type>();

                foreach (var eventHandler in eventHandlers) {
                    var type = eventHandler.GetInterfaces().FirstOrDefault(genEvntType.IsAssignableFrom);

                    if (type != null)
                        handlers.Add(eventHandler);
                }

                if (handlers.Count == 0) {
                    errors.Add(string.Format("No handlers for event type {0}", evnt.Name));
                    continue;
                }

                result.Add(evnt, handlers);
            }

            if (errors.Count > 0) {
                throw new Exception(string.Join("\r\n", errors));
            }

            return result;
        }
    }
}