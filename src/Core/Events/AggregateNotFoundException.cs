using System;

namespace DotNetKillboard.Events
{
    /// <summary>
    /// Exception thrown when an aggregate cannot be found by its identifier.
    /// </summary>
    public class AggregateNotFoundException : Exception
    {
    }
}