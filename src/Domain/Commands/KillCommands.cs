using System;
using System.Collections.Generic;

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
        public int AllianceId { get; set; }
        public int CorpId { get; set; }
        public int PilotId { get; set; }
        public int ShipId { get; set; }
        public int WeaponId { get; set; }

        public KillPartyParameter(decimal damageDone, decimal securityStatus, bool finalBlow, int allianceId, int corpId, int pilotId, int shipId, int weaponId) {
            DamageDone = damageDone;
            SecurityStatus = securityStatus;
            FinalBlow = finalBlow;
            AllianceId = allianceId;
            CorpId = corpId;
            PilotId = pilotId;
            ShipId = shipId;
            WeaponId = weaponId;
        }
    }

    public class KillItemParameter
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Location { get; set; }

        public KillItemParameter(int id, int quantity, int location) {
            Id = id;
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

        public DateTime KillDate { get; set; }

        public decimal DamageTaken { get; set; }

        public int KillPoints { get; set; }

        public decimal IskLoss { get; set; }

        public int ShipId { get; set; }

        public int SystemId { get; set; }

        public int AllianceId { get; set; }

        public int CorpId { get; set; }

        public int PilotId { get; set; }

        public IEnumerable<KillPartyParameter> InvolvedParties { get; set; }

        public IEnumerable<KillItemParameter> DestroyedItems { get; set; }

        public IEnumerable<KillItemParameter> DroppedItems { get; set; }

        public CreateKill(Guid id, int sequence, DateTime killDate, decimal damageTaken, int killPoints, decimal iskLoss,
            int shipId, int systemId, int allianceId, int corpId, int pilotId,
            IEnumerable<KillPartyParameter> involvedParties, IEnumerable<KillItemParameter> destroyedItems, IEnumerable<KillItemParameter> droppedItems)
            : base(id) {
            Sequence = sequence;
            KillDate = killDate;
            DamageTaken = damageTaken;
            KillPoints = killPoints;
            IskLoss = iskLoss;
            ShipId = shipId;
            SystemId = systemId;
            AllianceId = allianceId;
            CorpId = corpId;
            PilotId = pilotId;
            InvolvedParties = involvedParties;
            DestroyedItems = destroyedItems;
            DroppedItems = droppedItems;
        }
    }
}