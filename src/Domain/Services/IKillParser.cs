using System;

namespace DotNetKillboard.Services
{
    public interface IKillMailParser
    {
        bool Parse(string killmail);
    }

    public class KillMailException : Exception
    {
        public KillMailException(string message)
            : base(message) {

        }
    }
}