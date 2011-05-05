using System;
using DotNetKillboard.ReportingModel;
using DotNetKillboard.Services;
using DotNetKillboard.Services.Implementation;
using DotNetKillboard.Services.Model;
using NUnit.Framework;

namespace Tests.ServiceTest
{
    [TestFixture]
    public class KillServiceTest : MongoTestBase
    {

        protected override void OnSetup() {
            RegisterBus()
                .RegisterReportingRepository()
                .RegisterEntitiesServices()
                .RegisterFilters()
                .RegisterCommandsAndEvents();

            Resolver.Container.Register<IKillService, KillServiceImpl>();
        }

        [Test]
        public void Test1() {
            var system = new SolarSystemDto { Id = 1, Name = "SystemName" };
            Save(system);

            var kill = new ParsedKillResult {
                Header = new ParsedKillHeader {
                    AllianceName = "VictimAlliance",
                    CorporationName = "VictimCorp",
                    DamageTaken = 100,
                    FactionName = "FactionName",
                    ShipName = "VictimShip",
                    SystemName = system.Name,
                    SystemSecurity = 9000,
                    Timestamp = DateTime.Now,
                    VictimName = "VictimPilot"
                }
            };

            var ks = Resolve<IKillService>();
            ks.CreateKill(kill);
        }

    }
}