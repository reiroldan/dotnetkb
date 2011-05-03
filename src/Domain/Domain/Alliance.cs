using System;
using DotNetKillboard.Events;

namespace DotNetKillboard.Domain
{
    public class Alliance : AggregateRoot
    {
        private int _externalId;
        private string _name;
        private DateTime _timestamp;

        public Alliance() { }

        public Alliance(Guid id, string name, int externalId) {
            ApplyChange(new AllianceCreated(id, name, externalId, SystemDateTime.Now()));
        }

        #region Event Handlers

        protected void OnAllianceCreated(AllianceCreated e) {
            Id = e.Id;
            _externalId = e.ExternalId;
            _name = e.Name;
            _timestamp = e.Timestamp;
        }

        #endregion

    }
}