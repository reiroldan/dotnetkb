using System.Collections.Generic;

namespace DotNetKillboard.Services
{
    public class ParsedKillResult
    {
        private readonly List<ParsedInvolvedParty> _involvedParties;
        private readonly List<ParsedKillItem> _destroyedItems;
        private readonly List<ParsedKillItem> _droppedItems;

        public ParsedKillHeader Header { get; set; }

        public IEnumerable<ParsedInvolvedParty> InvolvedParties {
            get { return _involvedParties; }
        }

        public IEnumerable<ParsedKillItem> DestroyedItems {
            get { return _destroyedItems; }
        }

        public IEnumerable<ParsedKillItem> DroppedItems { get { return _droppedItems; } }

        public ParsedKillResult() {
            Header = new ParsedKillHeader();
            _involvedParties = new List<ParsedInvolvedParty>();
            _destroyedItems = new List<ParsedKillItem>();
            _droppedItems = new List<ParsedKillItem>();
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
    }
}