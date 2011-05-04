using System;
using System.Linq;
using DotNetKillboard.Data;
using DotNetKillboard.Data.Queries;
using DotNetKillboard.ReportingModel;
using DotNetKillboard.ReportingQueries;
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
                .RegisterEntitiesServices();

            Resolver.Container.Register<IKillService, KillServiceImpl>();

            var filters = FilterHelper.GetFilterTypes();

            foreach (var f in filters) {
                Resolver.Container.Register(f.Key, f.Value);
            }
        }

        [Test]
        public void Test1() {
            var kill = new ParsedKillResult {
                Header = new ParsedKillHeader {
                    AllianceName = "VictimAlliance",
                    CorporationName = "VictimCorp",
                    DamageTaken = 100,
                    FactionName = "FactionName",
                    ShipName = "VictimShip",
                    SystemName = "SystemName",
                    SystemSecurity = 9000,
                    Timestamp = DateTime.Now,
                    VictimName = "VictimPilot"
                }
            };

            var ks = Resolve<IKillService>();
            ks.CreateKill(kill);
        }

        private class TestSolarSystemByNameQuery : MongoQueryBase, ISolarSystemByNameQuery
        {
            public SolarSystemDto Execute() {
                return new SolarSystemDto { Name = Name };
            }

            public string Name { get; set; }
        }

    }
}