using System;
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

            if (!kill.Header.Timestamp.HasValue) {
                throw new KillMailException("Missing kill date information");
            }

            var killDate = kill.Header.Timestamp.Value;
            var victimSystem = _repository.QueryFor<ISolarSystemByNameQuery>(q => q.Name = kill.Header.SystemName).Execute();

            if (victimSystem == null) {
                throw new KillMailException("Missing victim's system {0}", kill.Header.SystemName);
            }

            var itemNames = kill.GetUsedItemNames();
            var involvedItems = _repository.QueryFor<IItemsWithNamesQuery>(n => n.Names = itemNames).Execute();
            var involvedItemDic = involvedItems.ToDictionary(x => x.Name, x => x);

            var victimAlliance = GetAlliance(kill.Header.AllianceName);
            var victimCorp = _entitiesService.GetCorporation(kill.Header.CorporationName, victimAlliance.Sequence);
            var victimPilot = _entitiesService.GetPilot(kill.Header.VictimName, victimAlliance.Sequence, victimCorp.Sequence);
            var victimShip = involvedItemDic[kill.Header.ShipName];
            var victimKillPoints = victimShip.Points;
            var victimIskLoss = victimShip.GetPrice();

            var involvedKillsPoints = 0;
            var maxKillPoints = Convert.ToDouble(Math.Round(victimKillPoints * 1.2m));

            var involvedParty = new List<KillPartyParameter>();

            foreach (var party in kill.InvolvedParties) {
                var partyAlliance = GetAlliance(party.AllianceName);
                var partyCorp = _entitiesService.GetCorporation(party.CorporationName, partyAlliance.Sequence);
                var partyPilot = _entitiesService.GetPilot(party.PilotName, partyAlliance.Sequence, partyCorp.Sequence, party.SecurityStatus);
                var partyShip = involvedItemDic[party.ShipName];
                var partyWeapon = involvedItemDic[party.WeaponName];

                var partyParam = new KillPartyParameter(
                    party.DamageDone,
                    party.SecurityStatus,
                    party.FinalBlow,
                    new KillIdSeqNameParameter(partyAlliance.Id, partyAlliance.Sequence, partyAlliance.Name),
                    new KillIdSeqNameParameter(partyCorp.Id, partyCorp.Sequence, partyCorp.Name),
                    new KillIdSeqNameParameter(partyPilot.Id, partyPilot.Sequence, partyPilot.Name),
                    new KillSimpleItemParameter(partyShip.Id, partyShip.Name),
                    new KillSimpleItemParameter(partyWeapon.Id, partyWeapon.Name)
                    );

                involvedKillsPoints += partyShip.Points;

                involvedParty.Add(partyParam);
            }

            var destroyedItems = new List<KillItemParameter>();

            foreach (var item in kill.DestroyedItems) {
                if (item.Name.Contains("Blueprint")) // Ignore blueprints
                    continue;

                var destroyedItem = involvedItemDic[item.Name];
                victimIskLoss += destroyedItem.GetPrice() * item.Quantity;

                var location = 0;
                //TODO: Get location
                destroyedItems.Add(new KillItemParameter(destroyedItem.Id, destroyedItem.Name, item.Quantity, location));
            }

            foreach (var item in kill.DroppedItems) {
                if (item.Name.Contains("Blueprint")) // Ignore blueprints
                    continue;

                var droppedItem = involvedItemDic[item.Name];
                victimIskLoss += droppedItem.GetPrice() * item.Quantity;
            }

            // Calculate kill points
            var killGankfactor = victimKillPoints / (victimKillPoints + involvedKillsPoints);
            var killPoints = Math.Ceiling(victimKillPoints * (killGankfactor / 0.75));

            if (killPoints > maxKillPoints)
                killPoints = maxKillPoints;

            killPoints = Math.Round(killPoints, 0);

            var killId = SystemIdGenerator.Next();
            var killSequence = _repository.GetNextSequenceFor<KillDto>();

            var createKillCommand = new CreateKill(killId,
                    killSequence,
                    killDate,
                    kill.Header.DamageTaken,
                    Convert.ToInt32(killPoints),
                    new KillSimpleItemParameter(victimShip.Id, victimShip.Name),
                    new KillSystemParameter(victimSystem.Id, victimSystem.Name),
                    new KillIdSeqNameParameter(victimAlliance.Id, victimAlliance.Sequence, victimAlliance.Name),
                    new KillIdSeqNameParameter(victimCorp.Id, victimCorp.Sequence, victimCorp.Name),
                    new KillIdSeqNameParameter(victimPilot.Id, victimPilot.Sequence, victimPilot.Name)
                );
        }

        #region Helpers

        private AllianceDto GetAlliance(string name) {
            return name == Constants.None
                ? new AllianceDto { Id = Guid.Empty, Sequence = 0, Name = name }
                : _entitiesService.GetAlliance(name);
        }

        #endregion
    }
}