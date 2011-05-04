using DotNetKillboard.Commands;
using DotNetKillboard.Domain;
using DotNetKillboard.Events;

namespace DotNetKillboard.CommandHandlers
{
    public class AllianceCommandHandlers : ICommandHandler<CreateAlliance>
    {
        private readonly IDomainRepository _repository;

        public AllianceCommandHandlers(IDomainRepository repository) {
            _repository = repository;
        }

        public void Handle(CreateAlliance command) {
            var item = new Alliance(command.Id, command.Sequence, command.Name, command.ExternalId);
            _repository.Save(item);
        }
    }
}