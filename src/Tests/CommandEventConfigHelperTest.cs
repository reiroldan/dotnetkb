using System.Linq;
using DotNetKillboard;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CommandEventConfigHelperTest
    {

        [Test]
        public void TestCommandsAndHandlers() {
            var commands = CommandEventConfigHelper.GetCommandAndHandlerTypes();
            commands.Select(x => new { Command = x.Key.Name, Handler = x.Value.Name }).Dump();
        }

        [Test]
        public void TestEventsAndHandlers() {
            var events = CommandEventConfigHelper.GetEventAndHandlerTypes();
            events.Select(x => new { Command = x.Key.Name, Handler = x.Value.Select(h => h.Name) }).Dump();
        }
    }
}