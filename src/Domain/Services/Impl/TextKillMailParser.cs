using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DotNetKillboard.Services.Impl
{
    public class TextKillMailParser : IKillMailParser
    {

        private string _killmail;

        public TextKillMailParser() {
            ParseErrors = new List<string>();
            Result = new ParsedKillResult();
        }

        public bool Parse(string killmail) {
            _killmail = killmail.Replace("\r", "").Trim();
            PerformSubstitutions();

            var headResult = ParseHeader();

            if (!headResult) {
                return false;
            }

            ParseInvolvedParties();
            ParseDestroyedItems();
            ParseDroppedItems();
            return true;
        }

        public ParsedKillResult Result { get; set; }

        public List<string> ParseErrors { get; private set; }

        private void PerformSubstitutions() {
            _killmail = _killmail
                .Replace("NONE", Constants.None)
                .Replace("UNKWON", Constants.Unkown);
        }

        private bool ParseHeader() {
            var involvedpos = _killmail.IndexOf("Involved parties:");

            if (involvedpos == -1) {
                AddError("Mail lacks Involved parties header.");
                return false;
            }

            var header = _killmail.Substring(0, involvedpos);

            DateTime timestamp;

            if (DateTime.TryParse(header.Substring(0, 16), out timestamp)) {
                Result.Header.Timestamp = timestamp;
            }

            var headerParts = _killmail.Substring(0, involvedpos).Trim().Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var moonName = "";
            var moonFound = false;

            foreach (var part in headerParts) {
                string tmp;

                if (Match("Victim: (.*)", part, out tmp)) {
                    Result.Header.VictimName = tmp;
                    continue;
                }

                if (Match("Corp: (.*)", part, out tmp)) {
                    Result.Header.CorporationName = tmp;
                    continue;
                }

                if (Match("Alliance: (.*)", part, out tmp)) {
                    Result.Header.AllianceName = tmp;
                    continue;
                }

                if (Match("Faction: (.*)", part, out tmp)) {
                    Result.Header.FactionName = tmp;
                    continue;
                }

                if (Match("Destroyed: (.*)", part, out tmp)) {
                    Result.Header.ShipName = tmp;
                    continue;
                }

                if (Match("System: (.*)", part, out tmp)) {
                    //bad assumption here - moon has to come before security.
                    Result.Header.SystemName = tmp;

                    if (moonName.InsensitiveCompare(Constants.Unkown) && moonFound) {
                        moonName = tmp;
                        Result.Header.VictimName = tmp;
                    }
                    continue;
                }

                if (Match("Security: (.*)", part, out tmp)) {
                    Result.Header.SystemSecurity = tmp.ToInvariantDecimal();
                    continue;
                }

                if (Match("Damage Taken: (.*)", part, out tmp)) {
                    Result.Header.DamageTaken = tmp.ToInvariantDecimal();
                    continue;
                }

                if (Match("Moon: (.*)", part, out tmp)) {
                    Result.Header.VictimName = tmp;
                    moonFound = true;
                    continue;
                }
            }

            if (moonFound) {
                Result.Header.VictimName = moonName;
            }

            if (Result.Header.AllianceName.InsensitiveCompare(Constants.None))
                Result.Header.AllianceName = Result.Header.FactionName;

            if (Result.Header.VictimName.InsensitiveCompare(Constants.Unkown)) {
                AddError("Victim has no name.");
                Result.Header.VictimName = "";
            }

            if (Result.Header.CorporationName.InsensitiveCompare(Constants.Unkown)) {
                AddError("Victim has no corp.");
                Result.Header.CorporationName = "";
            }

            if (Result.Header.ShipName.InsensitiveCompare(Constants.Unkown)) {
                AddError("Victim has no ship type.");
                Result.Header.ShipName = "";
            }

            if (Result.Header.SystemName.InsensitiveCompare(Constants.Unkown)) {
                AddError("Killmail lacks solar system information.");
                Result.Header.SystemName = "";
            }

            return Result.Header.Timestamp.HasValue && !Result.Header.FactionName.IsNullOrEmpty() 
                && !Result.Header.AllianceName.IsNullOrEmpty() && !Result.Header.CorporationName.IsNullOrEmpty() 
                && !Result.Header.VictimName.IsNullOrEmpty() && !Result.Header.ShipName.IsNullOrEmpty()
                && !Result.Header.SystemName.IsNullOrEmpty();
        }

        private void ParseInvolvedParties() {
            // involved parties section
            var end = _killmail.IndexOf("Destroyed items:");

            if (end == 0) {
                end = _killmail.IndexOf("Dropped items:");
                if (end == 0) {
                    //try to parse to the end of the mail in the event sections are missing
                    end = _killmail.Length;
                }
            }

            var involvedPosition = _killmail.IndexOf("Involved parties:") + 17;
            var involvedSection = _killmail.Substring(involvedPosition, end - involvedPosition).TrimStart();
            var involved = involvedSection.Split(new[] { "\n" }, StringSplitOptions.None);
            var groups = new List<List<string>>();
            var startPos = 0;

            for (var j = 0; j < involved.Length; j++) {

                var current = involved[j];
                var currentList = new List<string>();
                var atEnd = j + 1 == involved.Length;

                if (current != "" || (!atEnd && involved[j + 1] == ""))
                    continue;

                groups.Add(currentList);
                var k = startPos;

                while (k < j) {
                    var invPart = involved[k];

                    if (!invPart.IsNullOrEmpty())
                        currentList.Add(involved[k]);

                    k++;
                }

                startPos = j + 1;
            }

            for (var j = 0; j < groups.Count; j++) {
                var group = groups[j];
                var party = new ParsedInvolvedParty();

                foreach (var line in group) {
                    string tmp;

                    if (Match("Name: (.*)", line, out tmp)) {
                        var slash = line.IndexOf("/");

                        if (slash >= 0) {
                            var name = line.Substring(5, slash - 5).Trim();
                            var corporation = line.Substring(slash + 1, line.Length - slash + 1).Trim();

                            // now if the corp bit has final blow info, note it
                            if (corporation.IndexOf("laid the final blow") > 0) {
                                party.FinalBlow = true;
                                party.WeaponName = name;
                                end = corporation.IndexOf("(") - 1;
                                corporation = corporation.Substring(0, end);
                            } else {
                                party.WeaponName = name;
                            }
                            // alliance lookup for warp disruptors - normal NPCs aren't to be bundled in
                            //                        $crp = new Corporation();
                            //                        $crp->lookup($corporation);
                            //                        if($crp->getID() > 0 && ( stristr($name, ' warp ') || stristr($name, ' control ')))
                            //                        {
                            //                            $al = $crp->getAlliance();
                            //                            $ianame = $al->getName();
                            //                        }

                            //                        $ipname = $name;
                            //                        $icname = $corporation;
                        } else {
                            party.PilotName = tmp;
                            string tmpipname;
                            if (Match(@"(.*) \(laid the final blow\)", tmp, out tmpipname)) {
                                party.PilotName = tmpipname;
                                party.FinalBlow = true;
                            }
                        }

                        continue;
                    }

                    if (Match("Alliance: (.*)", line, out tmp)) {
                        party.AllianceName = tmp;
                        continue;
                    }

                    if (Match("Faction: (.*)", line, out tmp)) {
                        party.FactionName = tmp;
                        continue;
                    }

                    if (Match("Corp: (.*)", line, out tmp)) {
                        party.CorporationName = tmp;
                        continue;
                    }

                    if (Match("Ship: (.*)", line, out tmp)) {
                        party.ShipName = tmp;
                        continue;
                    }

                    if (Match("Weapon: (.*)", line, out tmp)) {
                        party.WeaponName = tmp;
                        continue;
                    }

                    if (Match("Security: (.*)", line, out tmp)) {
                        party.SecurityStatus = tmp.ToInvariantDecimal();
                        continue;
                    }

                    if (Match("Damage Done: (.*)", line, out tmp)) {
                        party.DamageDone = tmp.ToInvariantDecimal();
                        continue;
                    }
                }

                if (party.AllianceName.InsensitiveCompare(Constants.None)) {
                    party.AllianceName = party.FactionName;
                }

                if (party.CorporationName.InsensitiveCompare(Constants.None)) {
                    AddError(string.Format("Involved party has no corp. (Party No. {0})", j));
                }

                if (party.PilotName.InsensitiveCompare(Constants.Unkown)) {
                    if (party.WeaponName.IndexOf("Mobile") >= 0 || party.WeaponName.IndexOf("Control Tower") >= 0) {
                        //for involved parties parsed that lack a pilot, but are actually POS or mobile warp disruptors
                        party.PilotName = party.WeaponName;
                    } else {
                        AddError(string.Format("Involved party has no name. (Party No. {0})", j));
                    }
                }

                if (party.WeaponName.InsensitiveCompare(Constants.Unkown)) {
                    AddError(string.Format("No weapon found for pilot {0}.", party.PilotName));
                }

                Result.AddInvolvedParty(party);
            }
        }

        private void ParseDestroyedItems() {
            var destroyedpos = _killmail.IndexOf("Destroyed items:");

            if (destroyedpos < 0)
                return;

            var pos = _killmail.IndexOf("Dropped items:");

            if (pos == -1) {
                pos = _killmail.Length;
            }

            var endpos = pos - destroyedpos - 16;
            var destroyedSection = _killmail.Substring(destroyedpos + 16, endpos).Trim();
            var destroyed = destroyedSection.Split(new[] { "\n" }, StringSplitOptions.None);
            var items = ParseItems(destroyed);
            Result.AddDestroyedItems(items);
        }

        private void ParseDroppedItems() {
            var startpos = _killmail.IndexOf("Dropped items:");

            if (startpos < 0)
                return;

            var droppedSection = _killmail.Substring(startpos + 14).Trim();
            var dropped = droppedSection.Split(new[] { "\n" }, StringSplitOptions.None);
            var droppedItems = ParseItems(dropped);
            Result.AddDroppedItems(droppedItems);
        }

        private IEnumerable<ParsedKillItem> ParseItems(IEnumerable<string> destroyed) {
            var items = new List<ParsedKillItem>();

            foreach (var line in destroyed) {
                var current = line.Trim();
                var container = false;

                //API mod will return null when it can't lookup an item, so filter these
                if (current == "(Cargo)" || current.IndexOf(", Qty:") == 0) {
                    AddError(string.Format("Item name missing, yet item has quantity. Proposed name {0}", current));
                    continue;
                }

                if (current.IsNullOrEmpty()) {
                    continue;
                }

                if (current == "Empty.") {
                    continue;
                }

                var hasqty = current.Contains(", Qty: ");
                var qtypos = current.IndexOf(", Qty: ");
                var hasloc = current.Contains("(");
                var locpos = current.IndexOf("(");
                var quantity = "";
                var location = "";
                var itemlen = 0;
                var qtylen = 0;
                var loclen = 0;

                if (current.IndexOf("Container") >= 0) {
                    container = true;
                }

                if (!hasqty && !hasloc) {
                    itemlen = current.Length;
                    if (container)
                        location = "Cargo";
                }

                if (hasqty && !hasloc) {
                    itemlen = qtypos;
                    qtylen = current.Length - qtypos - 6;
                    if (container)
                        location = "Cargo";
                }

                if (hasloc && !hasqty) {
                    itemlen = locpos - 1;
                    qtylen = 0;
                    loclen = current.Length - locpos - 2;
                }

                if (hasloc && hasqty) {
                    itemlen = qtypos;
                    qtylen = locpos - qtypos - 7;
                    loclen = current.Length - locpos - 2;
                }

                var itemname = current.Substring(0, itemlen);

                if (qtypos >= 0)
                    quantity = current.Substring(qtypos + 6, qtylen);

                if (locpos >= 0)
                    location = current.Substring(locpos + 1, loclen);

                if (quantity == "") {
                    quantity = "1";
                }

                if (location == "In Container") {
                    location = "Cargo";
                }

                var item = new ParsedKillItem {
                    Name = itemname.Trim(),
                    Location = location,
                    Quantity = int.Parse(quantity)
                };

                items.Add(item);
            }

            return items;
        }

        #region Helpers

        private static bool Match(string pattern, string input, out string result, int position = 1) {
            var match = Regex.Match(input, pattern);
            result = match.Success ? match.Groups[position].Value : string.Empty;
            return match.Success;
        }

        private void AddError(string msg) {
            ParseErrors.Add(msg);
        }

        #endregion

    }
}