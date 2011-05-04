using DotNetKillboard.Bus;

namespace DotNetKillboard.Services.Impl
{
    public class KillServiceImpl : IKillService
    {
        private readonly IBus _bus;

        public KillServiceImpl(IBus bus) {
            _bus = bus;
        }

        public void CreateKill(ParsedKillResult kill) {
            if (kill.HasParseErrors) {
                var errors = string.Join("\r\n", kill.ParseErrors);
                throw new KillMailException(errors);
            }            
        }
    }
}