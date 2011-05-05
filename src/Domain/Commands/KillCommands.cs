using System;

namespace DotNetKillboard.Commands
{

    public class KillIdSeqNameParameter
    {
        public Guid Id { get; private set; }

        public int Sequence { get; private set; }

        public string Name { get; private set; }

        public KillIdSeqNameParameter(Guid id, int sequence, string name) {
            Id = id;
            Sequence = sequence;
            Name = name;
        }
    }

    public class KillSystemParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public KillSystemParameter(int id, string name) {
            Id = id;
            Name = name;
        }
    }

    public class KillPartyParameter
    {
        public decimal DamageDone { get; set; }
        public decimal SecurityStatus { get; set; }
        public bool FinalBlow { get; set; }
        public KillIdSeqNameParameter Alliance { get; set; }
        public KillIdSeqNameParameter Corp { get; set; }
        public KillIdSeqNameParameter Pilot { get; set; }
        public KillSimpleItemParameter Ship { get; set; }
        public KillSimpleItemParameter Weapon { get; set; }

        public KillPartyParameter(decimal damageDone, decimal securityStatus, bool finalBlow, KillIdSeqNameParameter alliance, KillIdSeqNameParameter corp, KillIdSeqNameParameter pilot, KillSimpleItemParameter ship, KillSimpleItemParameter weapon) {
            DamageDone = damageDone;
            SecurityStatus = securityStatus;
            FinalBlow = finalBlow;
            Alliance = alliance;
            Corp = corp;
            Pilot = pilot;
            Ship = ship;
            Weapon = weapon;
        }
    }

    public class KillItemParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Location { get; set; }

        public KillItemParameter(int id, string name, int quantity, int location) {
            Id = id;
            Name = name;
            Quantity = quantity;
            Location = location;
        }
    }

    public class KillSimpleItemParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public KillSimpleItemParameter(int id, string name) {
            Id = id;
            Name = name;
        }
    }

    public class CreateKill : CommandBase
    {
        public int Sequence { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal DamageTaken { get; set; }

        public int KillPoints { get; set; }

        public KillSimpleItemParameter Ship { get; set; }

        public KillSystemParameter System { get; set; }

        public KillIdSeqNameParameter Alliance { get; set; }

        public KillIdSeqNameParameter Corp { get; set; }

        public KillIdSeqNameParameter Pilot { get; set; }

        public CreateKill(Guid id, int sequence, DateTime timestamp, decimal damageTaken, int killPoints, KillSimpleItemParameter ship, KillSystemParameter system, KillIdSeqNameParameter alliance, KillIdSeqNameParameter corp, KillIdSeqNameParameter pilot)
            : base(id) {
            Sequence = sequence;
            Timestamp = timestamp;
            DamageTaken = damageTaken;
            KillPoints = killPoints;
            Ship = ship;
            System = system;
            Alliance = alliance;
            Corp = corp;
            Pilot = pilot;
        }
    }
}