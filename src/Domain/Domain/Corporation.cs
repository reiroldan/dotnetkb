using System;
using DotNetKillboard.Events;

namespace DotNetKillboard.Domain
{
    public class Corporation : AggregateRoot
    {
        private int _allianceId;
        private int _externalId;
        private string _name;
        private DateTime _timestamp;

        public Corporation() { }

        public Corporation(Guid id, string name, int allianceId, int externalId) {
            ApplyChange(new CorporatioCreated(id, name, allianceId, externalId, SystemDateTime.Now()));
        }

        #region Event Handlers

        protected void OnCorporatioCreated(CorporatioCreated e) {
            Id = e.Id;
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