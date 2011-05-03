using System;
using DotNetKillboard.Events;

namespace DotNetKillboard.Domain
{
    public class Pilot : AggregateRoot
    {
        private string _name;
        private int _allianceId;
        private int _corporationId;
        private int _externalId;
        private DateTime _timestamp;

        public Pilot() { }

        public Pilot(Guid id, string name, int allianceId, int corporationId, int externalId) {
            ApplyChange(new PilotCreated(id, name, allianceId, corporationId, externalId, SystemDateTime.Now()));
        }

        #region Event Handlers

        protected void OnPilotCreated(PilotCreated e) {
            Id = e.Id;
            _name = e.Name;
            _allianceId = e.AllianceId;
            _corporationId = e.CorporationId;
            _externalId = e.ExternalId;
            _timestamp = e.Timestamp;
        }

        protected void OnPilotAllianceCorporationChanged(PilotAllianceCorporationChanged e) {
            _corporationId = e.CorporationId;
            _timestamp = e.Timestamp;
        }

        #endregion

        #region Public Implementation

        public void ChangeAllianceAndCorporation(int allianceId, int corporationId) {
            ApplyChange(new PilotAllianceCorporationChanged(Id, allianceId, corporationId, SystemDateTime.Now()));
        }

        #endregion
    }
}