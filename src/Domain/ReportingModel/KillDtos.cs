using System;
using System.Collections.Generic;

namespace DotNetKillboard.ReportingModel
{
    public class KillDto
    {

        public decimal DamageTaken { get; set; }

        public Guid Id { get; set; }

        public decimal IskLoss { get; set; }

        public DateTime KillDate { get; set; }

        public int KillPoints { get; set; }

        public int Sequence { get; set; }

        public int ShipId { get; set; }

        public int SystemId { get; set; }

        public DateTime Timestamp { get; set; }

        public int VictimAllianceId { get; set; }

        public int VictimCorpId { get; set; }

        public int VictimPilotId { get; set; }

        public IList<KillInvolvedPartyDto> InvolvedParties { get; set; }

        public IList<KillItemDto> DestroyedItems { get; set; }

        public IList<KillItemDto> DroppedItems { get; set; }

        public KillDto() {
            InvolvedParties = new List<KillInvolvedPartyDto>();
            DestroyedItems = new List<KillItemDto>();
            DroppedItems = new List<KillItemDto>();
        }
    }

    public class KillInvolvedPartyDto
    {
        public int AllianceId { get; set; }
        public int CorpId { get; set; }
        public int PilotId { get; set; }
        public int ShipId { get; set; }
        public int WeaponId { get; set; }
        public decimal DamageDone { get; set; }
        public decimal SecurityStatus { get; set; }
        public decimal DamageDone1 { get; set; }

        public KillInvolvedPartyDto(int allianceId, int corpId, int pilotId, int shipId, int weaponId, decimal damageDone, decimal securityStatus, decimal damageDone1) {
            AllianceId = allianceId;
            CorpId = corpId;
            PilotId = pilotId;
            ShipId = shipId;
            WeaponId = weaponId;
            DamageDone = damageDone;
            SecurityStatus = securityStatus;
            DamageDone1 = damageDone1;
        }
    }

    public class KillItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int Location { get; set; }

        public KillItemDto(int itemId, int quantity, int location) {
            ItemId = itemId;
            Quantity = quantity;
            Location = location;
        }
    }
}