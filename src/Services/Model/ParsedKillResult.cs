using System.Collections.Generic;
using System.Linq;

namespace DotNetKillboard.Services.Model
{
    public class ParsedKillResult
    {
        private readonly List<ParsedInvolvedParty> _involvedParties;
        private readonly List<ParsedKillItem> _destroyedItems;
        private readonly List<ParsedKillItem> _droppedItems;
        private readonly List<string> _parseErrors;

        public ParsedKillHeader Header { get; set; }

        public IEnumerable<ParsedInvolvedParty> InvolvedParties {
            get { return _involvedParties; }
        }

        public IEnumerable<ParsedKillItem> DestroyedItems {
            get { return _destroyedItems; }
        }

        public IEnumerable<ParsedKillItem> DroppedItems { get { return _droppedItems; } }

        public IEnumerable<string> ParseErrors { get { return _parseErrors; } }

        public bool HasParseErrors {
            get { return _parseErrors.Count > 0; }
        }

        public ParsedKillResult() {
            Header = new ParsedKillHeader();
            _involvedParties = new List<ParsedInvolvedParty>();
            _destroyedItems = new List<ParsedKillItem>();
            _droppedItems = new List<ParsedKillItem>();
            _parseErrors = new List<string>();
        }

        public void AddInvolvedParty(ParsedInvolvedParty party) {
            _involvedParties.Add(party);
        }

        public void AddDestroyedItems(IEnumerable<ParsedKillItem> destroyedItems) {
            _destroyedItems.AddRange(destroyedItems);
        }

        public void AddDroppedItems(IEnumerable<ParsedKillItem> droppedItems) {
            _droppedItems.AddRange(droppedItems);
        }

        public void AddParseError(string error) {
            _parseErrors.Add(error);
        }
    
        /// <summary>
        /// Gets a list of distinct item names that are involved in this kill
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetUsedItemNames() {
            var itemNames = new List<string> { Header.ShipName };
            itemNames.AddRange(DestroyedItems.Select(item => item.Name));
            itemNames.AddRange(DroppedItems.Select(item => item.Name));
            itemNames.AddRange(InvolvedParties.Select(item => item.ShipName));
            itemNames.AddRange(InvolvedParties.Select(item => item.WeaponName));
            
            itemNames.Remove(Constants.None);
            itemNames.Remove(Constants.Unkown);

            return itemNames.Distinct();
        }
    }
}