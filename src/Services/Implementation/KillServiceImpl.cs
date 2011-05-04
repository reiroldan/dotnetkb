using System.Collections.Generic;
using System.Linq;
using DotNetKillboard.Bus;
using DotNetKillboard.Reporting;
using DotNetKillboard.ReportingQueries;
using DotNetKillboard.Services.Model;

namespace DotNetKillboard.Services.Implementation
{
    public class KillServiceImpl : IKillService
    {
        private readonly IBus _bus;
        private readonly IReportingRepository _repository;
        private readonly IEntitiesService _entitiesService;

        public KillServiceImpl(IBus bus, IReportingRepository repository, IEntitiesService entitiesService) {
            _bus = bus;
            _repository = repository;
            _entitiesService = entitiesService;
        }

        public void CreateKill(ParsedKillResult kill) {
            if (kill.HasParseErrors) {
                var errors = string.Join("\r\n", kill.ParseErrors);
                throw new KillMailException(errors);
            }

            var victimSystem = _repository.QueryFor<ISolarSystemByNameQuery>(q => q.Name = kill.Header.SystemName).Execute();

            if (victimSystem == null) {
                throw new KillMailException("Missing victim's system {0}", kill.Header.SystemName);
            }

            var victimAlliance = _entitiesService.GetAlliance(kill.Header.AllianceName);
            var victimCorp = _entitiesService.GetCorporation(kill.Header.CorporationName, victimAlliance.Sequence);
            var victimPilot = _entitiesService.GetPilot(kill.Header.VictimName, victimAlliance.Sequence, victimCorp.Sequence);

            // Build names of all items involved
            var itemNames = new List<string> { kill.Header.ShipName };
            itemNames.AddRange(kill.DestroyedItems.Select(item => item.Name));
            itemNames.AddRange(kill.DroppedItems.Select(item => item.Name));
            itemNames.AddRange(kill.InvolvedParties.Select(item => item.ShipName));
            itemNames.AddRange(kill.InvolvedParties.Select(item => item.WeaponName));

            var itemsNamedQuery = _repository.QueryFor<IItemsWithNamesQuery>();

            itemsNamedQuery.Names = itemNames.Distinct();

            var involvedItems = itemsNamedQuery.Execute();

            foreach (var party in kill.InvolvedParties) {
                var partyAlliance = _entitiesService.GetAlliance(party.AllianceName);
                var partyCorp = _entitiesService.GetCorporation(party.CorporationName, partyAlliance.Sequence);
                var partyPilot = _entitiesService.GetPilot(party.PilotName, partyAlliance.Sequence, partyCorp.Sequence);
            }

        }
    }
}