using System;

namespace DotNetKillboard.Services
{
    public interface IKillMailParser
    {
        void Parse(string killmail, bool checkAuthorization = true);
    }

    public class KillMailException : Exception
    {
        public KillMailException(string message)
            : base(message) {

        }
    }
}