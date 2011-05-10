using DotNetKillboard.Events;
using DotNetKillboard.Reporting;
using DotNetKillboard.ReportingModel;

namespace DotNetKillboard.EventHandlers
{
    public class KillEventHandlers : IEventHandler<KillCreated>, IEventHandler<KillPartyAdded>,
        IEventHandler<KillDroppedItemAdded>, IEventHandler<KillDestroyedItemAdded>
    {
        private readonly IReportingRepository _repository;

        public KillEventHandlers(IReportingRepository repository) {
            _repository = repository;
        }

        public void Handle(KillCreated e) {
            var kill = new KillDto {
                DamageTaken = e.DamageTaken,
                Id = e.Id,
                IskLoss = e.IskLoss,
                KillDate = e.KillDate,
                KillPoints = e.KillPoints,
                Sequence = e.Sequence,
                ShipId = e.ShipId,
                SystemId = e.SystemId,
                Timestamp = e.Timestamp,
                VictimAllianceId = e.AllianceId,
                VictimCorpId = e.CorpId,
                VictimPilotId = e.PilotId
            };

            _repository.Save(kill);
        }

        public void Handle(KillPartyAdded e) {
            var kill = _repository.Get<KillDto>(e.Id);
            kill.InvolvedParties.Add(new KillInvolvedPartyDto(e.AllianceId, e.CorpId, e.PilotId, e.ShipId, e.WeaponId, e.DamageDone, e.SecurityStatus, e.DamageDone));
            _repository.Save(kill);
        }

        public void Handle(KillDroppedItemAdded e) {
            var kill = _repository.Get<KillDto>(e.Id);
            kill.DroppedItems.Add(new KillItemDto(e.ItemId, e.Quantity, e.Location));
            _repository.Save(kill);
        }

        public void Handle(KillDestroyedItemAdded e) {
            var kill = _repository.Get<KillDto>(e.Id);
            kill.DestroyedItems.Add(new KillItemDto(e.ItemId, e.Quantity, e.Location));
            _repository.Save(kill);
        }
    }
}