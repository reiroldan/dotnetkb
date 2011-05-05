using System;
using DotNetKillboard.Events;

namespace DotNetKillboard.Domain
{
    public class Corporation : AggregateRoot
    {
        private int _sequence;
        private int _allianceId;
        private int _externalId;
        private string _name;
        private DateTime _timestamp;
        
        public Corporation() { }

        public Corporation(Guid id, int sequence, string name, int allianceId, int externalId) {
            ApplyChange(new CorporationCreated(id, sequence, name, allianceId, externalId, SystemDateTime.Now()));
        }

        #region Event Handlers

        protected void OnCorporationCreated(CorporationCreated e) {
            Id = e.Id;
            _sequence = e.Sequence;
            _allianceId = e.AllianceId;
            _externalId = e.ExternalId;
            _name = e.Name;
            _timestamp = e.Timestamp;
        }

        protected void OnCorporationAllianceChanged(CorporationAllianceChanged e) {
            _allianceId = e.AllianceId;
            _timestamp = e.Timestamp;
        }

        #endregion

        public void ChangeAlliance(int allianceId) {
            ApplyChange(new CorporationAllianceChanged(Id, allianceId, SystemDateTime.Now()));
        }
    }
}