using System;

namespace DotNetKillboard.Services
{
    public class KillMailException : Exception
    {
        public KillMailException(string message)
            : base(message) {
        }

        public KillMailException(string message, params object[] args)
            : base(string.Format(message, args)) {

        }
    }
}