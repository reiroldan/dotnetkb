using System;

namespace DotNetKillboard.Events
{

    /// <summary>
    /// Base class for synchroneous events
    /// </summary>
    public class EventBase : IEvent
    {
        public Guid Id { get; protected set; }

        public int Version { get; set; }

        public bool Async { get { return false; } }

        public EventBase(Guid id) {
            Id = id;        
        }        
    }
}