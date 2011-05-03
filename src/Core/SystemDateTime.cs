using System;

namespace DotNetKillboard
{
    public static class SystemDateTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;

        public static void Reset() {
            Now = () => DateTime.Now;
        }
    }
}