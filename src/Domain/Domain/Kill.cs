using System;
using System.Collections.Generic;
using DotNetKillboard.Events;

namespace DotNetKillboard.Domain
{
    public class Kill : AggregateRoot
    {
        private int _sequence;
        private DateTime _killDate;
        private DateTime _timestamp;
        private decimal _damageTaken;
        private decimal _iskLoss;
        private int _killPoints;
        private int _allianceId;
        private int _corpId;
        private int _pilotId;
        private int _shipId;
        private int _systemId;
        private List<KillParty> _involvedParties;
        private List<KillItem> _destroyedItems;
        private List<KillItem> _droppedItems;

        public Kill() { }

        public Kill(Guid id, int sequence, DateTime killDate, int systemId, int shipId, decimal damageTaken, int killPoints, decimal iskLoss, int allianceId, int corpId, int pilotId) {
            ApplyChange(new KillCreated(id, sequence, killDate, systemId, shipId, damageTaken, killPoints, iskLoss, allianceId, corpId, pilotId, SystemDateTime.Now()));
        }

        #region Events

        protected void OnKillCreated(KillCreated e) {
            Id = e.Id;
            _sequence = e.Sequence;
            _killDate = e.KillDate;
            _timestamp = e.Timestamp;
            _damageTaken = e.DamageTaken;
            _iskLoss = e.IskLoss;
            _killPoints = e.KillPoints;
            _shipId = e.ShipId;
            _systemId = e.SystemId;
            _allianceId = e.AllianceId;
            _corpId = e.CorpId;
            _pilotId = e.PilotId;
            _involvedParties = new List<KillParty>();
            _droppedItems = new List<KillItem>();
            _destroyedItems = new List<KillItem>();
        }

        protected void OnKillPartyAdded(KillPartyAdded e) {
            _involvedParties.Add(new KillParty(e.AllianceId, e.CorpId, e.PilotId, e.ShipId, e.WeaponId, e.DamageDone, e.SecurityStatus, e.FinalBlow));
            _timestamp = e.Timestamp;
        }

        protected void OnKillDroppedItemAdded(KillDroppedItemAdded e) {
            _droppedItems.Add(new KillItem(e.ItemId, e.Quantity, e.Location));
            _timestamp = e.Timestamp;
        }

        protected void OnKillDestroyedItemAdded(KillDestroyedItemAdded e) {
            _destroyedItems.Add(new KillItem(e.ItemId, e.Quantity, e.Location));
            _timestamp = e.Timestamp;
        }

        #endregion

        public void AddParty(int allianceId, int corpId, int pilotId, decimal damageDone, int shipId, int weaponId, decimal securityStatus, bool finalBlow) {
            ApplyChange(new KillPartyAdded(Id, allianceId, corpId, pilotId, damageDone, shipId, weaponId, securityStatus, finalBlow, SystemDateTime.Now()));
        }

        public void AddDroppedItem(int itemId, int quantity, int location) {
            ApplyChange(new KillDroppedItemAdded(Id, itemId, quantity, location, SystemDateTime.Now()));
        }

        public void AddDestroyedItem(int itemId, int quantity, int location) {
            ApplyChange(new KillDestroyedItemAdded(Id, itemId, quantity, location, SystemDateTime.Now()));
        }
    }

    public class KillParty
    {
        public int AllianceId { get; set; }
        public int CorpId { get; set; }
        public int PilotId { get; set; }
        public int ShipId { get; set; }
        public int WeaponId { get; set; }
        public decimal DamageDone { get; set; }
        public decimal SecurityStatus { get; set; }
        public bool FinalBlow { get; set; }

        public KillParty(int allianceId, int corpId, int pilotId, int shipId, int weaponId, decimal damageDone, decimal securityStatus, bool finalBlow) {
            AllianceId = allianceId;
            CorpId = corpId;
            PilotId = pilotId;
            ShipId = shipId;
            WeaponId = weaponId;
            DamageDone = damageDone;
            SecurityStatus = securityStatus;
            FinalBlow = finalBlow;
        }
    }

    public class KillItem
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int Location { get; set; }

        public KillItem(int itemId, int quantity, int location) {
            ItemId = itemId;
            Quantity = quantity;
            Location = location;
        }
    }
}