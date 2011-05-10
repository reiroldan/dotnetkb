using System;

namespace DotNetKillboard.Events
{
    public class KillCreated : EventBase
    {
        public int Sequence { get; set; }
        public DateTime KillDate { get; set; }
        public int SystemId { get; set; }
        public int ShipId { get; set; }
        public decimal DamageTaken { get; set; }
        public int KillPoints { get; set; }
        public decimal IskLoss { get; set; }
        public int AllianceId { get; set; }
        public int CorpId { get; set; }
        public int PilotId { get; set; }
        public DateTime Timestamp { get; set; }

        public KillCreated(Guid id, int sequence, DateTime killDate, int systemId, int shipId, decimal damageTaken,
            int killPoints, decimal iskLoss, int allianceId, int corpId, int pilotId, DateTime timestamp)
            : base(id) {
            Sequence = sequence;
            KillDate = killDate;
            SystemId = systemId;
            ShipId = shipId;
            DamageTaken = damageTaken;
            KillPoints = killPoints;
            IskLoss = iskLoss;
            AllianceId = allianceId;
            CorpId = corpId;
            PilotId = pilotId;
            Timestamp = timestamp;
        }
    }

    public class KillPartyAdded : AsyncEventBase
    {
        public int AllianceId { get; set; }
        public int CorpId { get; set; }
        public int PilotId { get; set; }
        public decimal DamageDone { get; set; }
        public int ShipId { get; set; }
        public int WeaponId { get; set; }
        public decimal SecurityStatus { get; set; }
        public bool FinalBlow { get; set; }
        public DateTime Timestamp { get; set; }

        public KillPartyAdded(Guid id, int allianceId, int corpId, int pilotId, decimal damageDone, int shipId, int weaponId, decimal securityStatus, bool finalBlow, DateTime timestamp)
            : base(id) {
            AllianceId = allianceId;
            CorpId = corpId;
            PilotId = pilotId;
            DamageDone = damageDone;
            ShipId = shipId;
            WeaponId = weaponId;
            SecurityStatus = securityStatus;
            FinalBlow = finalBlow;
            Timestamp = timestamp;
        }
    }

    public class KillDroppedItemAdded : AsyncEventBase
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int Location { get; set; }
        public DateTime Timestamp { get; set; }

        public KillDroppedItemAdded(Guid id, int itemId, int quantity, int location, DateTime timestamp)
            : base(id) {
            ItemId = itemId;
            Quantity = quantity;
            Location = location;
            Timestamp = timestamp;
        }
    }

    public class KillDestroyedItemAdded : AsyncEventBase
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int Location { get; set; }
        public DateTime Timestamp { get; set; }

        public KillDestroyedItemAdded(Guid id, int itemId, int quantity, int location, DateTime timestamp)
            : base(id) {
            ItemId = itemId;
            Quantity = quantity;
            Location = location;
            Timestamp = timestamp;
        }
    }
}