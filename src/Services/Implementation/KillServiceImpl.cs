using System.Collections.Generic;
using System.Linq;
using DotNetKillboard.Bus;
using DotNetKillboard.Commands;
using DotNetKillboard.Reporting;
using DotNetKillboard.ReportingModel;
using DotNetKillboard.ReportingQueries;
using DotNetKillboard.Services.Model;

namespace DotNetKillboard.Services.Implementation
{
    public class KillServiceImpl : IKillService
    {
        private readonly IBus _bus;
        private readonly IReportingRepository _repository;

        public KillServiceImpl(IBus bus, IReportingRepository repository) {
            _bus = bus;
            _repository = repository;
        }

        public void CreateKill(ParsedKillResult kill) {
            if (kill.HasParseErrors) {
                var errors = string.Join("\r\n", kill.ParseErrors);
                throw new KillMailException(errors);
            }

            var allianceNameQuery = _repository.QueryFor<IAllianceByNameQuery>();
            var corpNameQuery = _repository.QueryFor<ICorporationByNameQuery>();
            var pilotNameQuery = _repository.QueryFor<IPilotByNameQuery>();
            var systemNameQuery = _repository.QueryFor<ISolarSystemByNameQuery>();
            var itemsNamedQuery = _repository.QueryFor<IItemsWithNamesQuery>();

            systemNameQuery.Name = kill.Header.SystemName;
            var victimSystem = systemNameQuery.Execute();

            if (victimSystem == null) {
                throw new KillMailException("Missing victim's system {0}", kill.Header.SystemName);
            }

            allianceNameQuery.Name = kill.Header.AllianceName;

            var victimAlliance = allianceNameQuery.Execute();
            var victimAllianceSequence = victimAlliance != null ? victimAlliance.Sequence : 0;

            // aliance has to be created
            if (victimAlliance == null) {
                victimAllianceSequence = _repository.GetNextSequenceFor<AllianceDto>();
                var uid = SystemIdGenerator.Next();
                _bus.Send(new CreateAlliance(uid, victimAllianceSequence, kill.Header.AllianceName, 0));
            }

            corpNameQuery.Name = kill.Header.CorporationName;

            var victimCorp = corpNameQuery.Execute();
            
            // corp has to be created
            if (victimCorp == null) {

            }

            pilotNameQuery.Name = kill.Header.VictimName;

            var victimPilot = pilotNameQuery.Execute();

            // pilot has to be created
            if (victimPilot == null) {

            }

            // Build names of all items involved
            var itemNames = new List<string> { kill.Header.ShipName };
            itemNames.AddRange(kill.DestroyedItems.Select(item => item.Name));
            itemNames.AddRange(kill.DroppedItems.Select(item => item.Name));
            itemNames.AddRange(kill.InvolvedParties.Select(item => item.ShipName));
            itemNames.AddRange(kill.InvolvedParties.Select(item => item.WeaponName));

            itemsNamedQuery.Names = itemNames.Distinct();

            var involvedItems = itemsNamedQuery.Execute();

            foreach (var party in kill.InvolvedParties) {
                allianceNameQuery.Name = party.AllianceName;

                var partyAlliance = allianceNameQuery.Execute();

                // aliance has to be created
                if (partyAlliance == null) {

                }

                corpNameQuery.Name = party.CorporationName;

                var partyCorp = corpNameQuery.Execute();

                // corp has to be created
                if (partyCorp == null) {

                }

                pilotNameQuery.Name = party.PilotName;

                var partyPilot = pilotNameQuery.Execute();

                // pilot has to be created
                if (partyPilot == null) {

                }
            }

        }
    }
}