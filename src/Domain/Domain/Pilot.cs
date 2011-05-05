using System;
using DotNetKillboard.Events;

namespace DotNetKillboard.Domain
{
    public class Pilot : AggregateRoot
    {
        private int _sequence;
        private string _name;
        private int _allianceId;
        private int _corporationId;
        private int _externalId;
        private DateTime _timestamp;

        public Pilot() { }

        public Pilot(Guid id, int sequence, string name, int allianceId, int corporationId, int externalId) {
            ApplyChange(new PilotCreated(id, sequence, name, allianceId, corporationId, externalId, SystemDateTime.Now()));
        }

        #region Event Handlers

        protected void OnPilotCreated(PilotCreated e) {
            Id = e.Id;
            _sequence = e.Sequence;
            _name = e.Name;
            _allianceId = e.AllianceId;
            _corporationId = e.CorporationId;
            _externalId = e.ExternalId;
            _timestamp = e.Timestamp;
        }

        protected void OnPilotCorporationChanged(PilotCorporationChanged e) {
            _corporationId = e.CorporationId;
            _timestamp = e.Timestamp;
        }

        protected void OnPilotAllianceChanged(PilotAllianceChanged e) {
            _allianceId = e.AllianceId;
            _timestamp = e.Timestamp;
        }

        #endregion

        #region Public Implementation

        public void ChangeCorporation(int corporationId) {
            ApplyChange(new PilotCorporationChanged(Id, corporationId, SystemDateTime.Now()));
        }

        #endregion

        public void ChangeAlliance(int allianceId) {
            ApplyChange(new PilotAllianceChanged(Id, allianceId, SystemDateTime.Now()));
        }
    }
}