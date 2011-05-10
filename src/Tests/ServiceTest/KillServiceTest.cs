using System;
using System.Linq;
using DotNetKillboard.ReportingModel;
using DotNetKillboard.Services;
using DotNetKillboard.Services.Implementation;
using DotNetKillboard.Services.Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests.ServiceTest
{
    [TestFixture]
    public class KillServiceTest : MongoTestBase
    {

        #region Mails

        public const string Kill = @"2011.05.03 14:45

Victim: The Victim
Corp: Victim Corporation
Alliance: Victim Alliance
Faction: NONE
Destroyed: Drake
System: Victim System
Security: 0.0
Damage Taken: 28178

Involved parties:

Name: Involved Party 1
Security: 3.8
Corp: Invovled Corp 1
Alliance: None
Faction: NONE
Ship: Vagabond
Weapon: Vagabond
Damage Done: 8660

Name: Involved Party 2
Security: 5.0
Corp: Invovled Corp 2
Alliance: None
Faction: NONE
Ship: Hurricane
Weapon: Hurricane
Damage Done: 2845

Name: Involved Party 3
Security: 2.6
Corp: Involved Corp 3
Alliance: None
Faction: NONE
Ship: Sleipnir
Weapon: Sleipnir
Damage Done: 2793

Name: Involved Party 4
Security: 5.0
Corp: Involved Corp 4
Alliance: None
Faction: NONE
Ship: Vagabond
Weapon: Vagabond
Damage Done: 2519

Name: Involved Party 5
Security: 4.7
Corp: Involved Corp 5
Alliance: None
Faction: NONE
Ship: Capsule
Weapon: Gremlin Rage Rocket
Damage Done: 2062

Name: Involved Party 6
Security: 4.6
Corp: Involved Corp 6
Alliance: None
Faction: NONE
Ship: Drake
Weapon: Terror Rage Assault Missile
Damage Done: 1768

Name: Involved Party 7
Security: 5.0
Corp: Involved Corp 7
Alliance: None
Faction: NONE
Ship: Drake
Weapon: Scourge Fury Heavy Missile
Damage Done: 1582

Name: Involved Party 8
Security: 5.0
Corp: Involved Corp 8
Alliance: Involved Alliance 8
Faction: NONE
Ship: Drake
Weapon: Thunderbolt Heavy Missile
Damage Done: 944

Name: Involved Party 9 Final Blow (laid the final blow)
Security: 0.8
Corp: Involved Corp 9
Alliance: None
Faction: NONE
Ship: Hurricane
Weapon: 425mm AutoCannon II
Damage Done: 858

Destroyed items:

Heavy Missile Launcher II, Qty: 4
Hydra F.O.F. Heavy Missile I, Qty: 17
Scourge Precision Heavy Missile, Qty: 25
Invulnerability Field II
Warp Disruptor II
Power Diagnostic System II
Ballistic Control System II
Scourge Precision Heavy Missile, Qty: 1465 (Cargo)
Havoc Precision Heavy Missile, Qty: 1000 (Cargo)
Power Diagnostic System II (Cargo)
Thunderbolt Fury Heavy Missile, Qty: 1500 (Cargo)
Scourge Fury Heavy Missile, Qty: 1000 (Cargo)
Medium Anti-EM Screen Reinforcer I
Medium Core Defence Field Extender I, Qty: 2

Dropped items:

Hydra F.O.F. Heavy Missile I, Qty: 44
Heavy Missile Launcher II, Qty: 2
Scourge Precision Heavy Missile, Qty: 52
Drone Link Augmentor I
Salvager I
Y-T8 Overcharged Hydrocarbon I Microwarpdrive
Invulnerability Field II
Large Shield Extender II, Qty: 2
Damage Control II
Co-Processor II
Hydra F.O.F. Heavy Missile I, Qty: 617 (Cargo)
";

        #endregion

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
            var shipItem = new ItemDto { Id = 1, Name = "VictimShip" };

            Save(system);
            Save(shipItem);

            var kill = new ParsedKillResult {
                Header = new ParsedKillHeader {
                    AllianceName = "VictimAlliance",
                    CorporationName = "VictimCorp",
                    DamageTaken = 100,
                    FactionName = "FactionName",
                    ShipName = shipItem.Name,
                    SystemName = system.Name,
                    SystemSecurity = 9000,
                    Timestamp = DateTime.Now,
                    VictimName = "VictimPilot"
                }
            };

            var ks = Resolve<IKillService>();
            ks.CreateKill(kill);
        }

        [Test]
        public void Test2() {
            var system = new SolarSystemDto { Id = 1, Name = "SystemName" };
            var shipItem = new ItemDto { Id = 1, Name = "VictimShip" };

            Save(system);
            Save(shipItem);

            var kill = new ParsedKillResult {
                Header = new ParsedKillHeader {
                    AllianceName = "VictimAlliance",
                    CorporationName = "VictimCorp",
                    DamageTaken = 100,
                    FactionName = "FactionName",
                    ShipName = shipItem.Name,
                    SystemName = system.Name,
                    SystemSecurity = 9000,
                    Timestamp = DateTime.Now,
                    VictimName = "VictimPilot"
                }
            };

            var ks = Resolve<IKillService>();
            ks.CreateKill(kill);

            kill.Header.AllianceName = "SecondAlliance";
            kill.Header.VictimName = "VictimPilot";

            ks.CreateKill(kill);
        }

        [Test]
        public void FullTest() {
            var parser = new TextKillMailParser();
            parser.Parse(Kill);
            var kill = parser.Result;

            Save(new SolarSystemDto { Id = 1, Name = kill.Header.SystemName });

            var itemNames = kill.GetUsedItemNames().ToList();

            for (var i = 0; i < itemNames.Count; i++) {
                Save(new ItemDto { Id = i + 1, Name = itemNames[i] });
            }

            var ks = Resolve<IKillService>();
            ks.CreateKill(kill);
        }

        [Test]
        public void MultiRun() {
            for (var i = 0; i < 1000; i++) {
                FullTest();
            }
        }

    }
}