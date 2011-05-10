using DotNetKillboard.Commands;
using DotNetKillboard.Domain;
using DotNetKillboard.Events;

namespace DotNetKillboard.CommandHandlers
{
    public class KillCommandHandlers : ICommandHandler<CreateKill>
    {
        private readonly IDomainRepository _repository;

        public KillCommandHandlers(IDomainRepository repository) {
            _repository = repository;
        }

        public void Handle(CreateKill command) {
            var kill = new Kill(command.Id, command.Sequence, command.KillDate, command.SystemId,
                command.ShipId, command.DamageTaken,
                command.KillPoints, command.IskLoss, command.AllianceId, command.CorpId, command.PilotId);

            foreach (var party in command.InvolvedParties) {
                kill.AddParty(party.AllianceId, party.CorpId, party.PilotId, party.DamageDone, party.ShipId, party.WeaponId, party.SecurityStatus, party.FinalBlow);
            }

            foreach (var item in command.DroppedItems) {
                kill.AddDroppedItem(item.Id, item.Quantity, item.Location);
            }

            foreach (var item in command.DestroyedItems) {
                kill.AddDestroyedItem(item.Id, item.Quantity, item.Location);
            }    
        
            _repository.Save(kill);            
        }
    }
}