using System;
using DotNetKillboard.Services;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DefaultKillParserTest
    {

        #region Mails

        public const string Kill = @"2011.05.03 14:45

Victim: eryyxx 2
Corp: Rubbish and Garbage Removal
Alliance: Bang Bang You're Dead
Faction: NONE
Destroyed: Drake
System: RPS-0K
Security: 0.0
Damage Taken: 28178

Involved parties:

Name: Thunder ebon
Security: 3.8
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Vagabond
Weapon: Vagabond
Damage Done: 8660

Name: No 002
Security: 5.0
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Hurricane
Weapon: Hurricane
Damage Done: 2845

Name: kanathor
Security: 2.6
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Sleipnir
Weapon: Sleipnir
Damage Done: 2793

Name: Thunder knife
Security: 5.0
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Vagabond
Weapon: Vagabond
Damage Done: 2519

Name: Thunder andy
Security: 4.7
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Capsule
Weapon: Gremlin Rage Rocket
Damage Done: 2062

Name: Peninsula Media
Security: 4.6
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Drake
Weapon: Terror Rage Assault Missile
Damage Done: 1768

Name: Lonely Devil
Security: 5.0
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Drake
Weapon: Scourge Fury Heavy Missile
Damage Done: 1582

Name: Zektyn
Security: 3.6
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Hurricane
Weapon: Hurricane
Damage Done: 1403

Name: LIZARD MK
Security: 5.0
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Manticore
Weapon: 'Arbalest' Siege Missile Launcher
Damage Done: 1384

Name: Damien Deaf
Security: 5.0
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Vagabond
Weapon: Vagabond
Damage Done: 1196

Name: Lincoln Chang
Security: 5.0
Corp: Black Thorne Corporation
Alliance: Black Thorne Alliance
Faction: NONE
Ship: Drake
Weapon: Thunderbolt Heavy Missile
Damage Done: 944

Name: FlyingBee (laid the final blow)
Security: 0.8
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Hurricane
Weapon: 425mm AutoCannon II
Damage Done: 858

Name: cpt hongkai
Security: 5.0
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Sabre
Weapon: Sabre
Damage Done: 164

Name: Thunder sop
Security: 0.9
Corp: Thunder Mercenary Army
Alliance: None
Faction: NONE
Ship: Curse
Weapon: Medium Energy Neutralizer II
Damage Done: 0


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

        [Test]
        public void Parse1() {
            var parser = new DefaultKillMailParser();
            parser.Parse(Kill);

            Console.WriteLine(parser.Result);
        }
    }
}