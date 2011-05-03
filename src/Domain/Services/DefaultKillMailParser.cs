using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DotNetKillboard.Services
{

    public class KillHeader
    {
        public DateTime? Timestamp { get; set; }

        public string VictimName { get; set; }

        public string CorporationName { get; set; }

        public string AllianceName { get; set; }

        public string FactionName { get; set; }

        public string ShipName { get; set; }

        public string MoonName { get; set; }

        public string SystemName { get; set; }

        public decimal SystemSecurity { get; set; }

        public decimal DamageTaken { get; set; }
    }

    public class KillInvolvedParty
    {
        public string PilotName { get; set; }

        public string CorporationName { get; set; }

        public string AllianceName { get; set; }

        public decimal SecurityStatus { get; set; }

        public string ShipName { get; set; }

        public string WeaponName { get; set; }

        public decimal DamageDone { get; set; }

        public bool FinalBlow { get; set; }
    }

    public class KillParseResult
    {
        public KillHeader Header { get; set; }

        public bool Authorized { get; set; }

        public List<KillInvolvedParty> InvolvedParties { get; private set; }

        public KillParseResult() {
            Header = new KillHeader();
            InvolvedParties = new List<KillInvolvedParty>();
        }

        public void AddInvolvedParty(KillInvolvedParty party) {
            InvolvedParties.Add(party);
        }
    }

    public class DefaultKillMailParser : IKillMailParser
    {

        private const string _unkown = "Unkown";
        private const string _none = "None";
        private string _killmail;

        public KillParseResult Result { get; set; }

        public List<string> ParseErrors { get; private set; }

        public DefaultKillMailParser() {
            ParseErrors = new List<string>();
            Result = new KillParseResult();
        }

        public void Parse(string killmail, bool checkAuthorization = true) {
            _killmail = killmail.Replace("\r", "").Trim();

            ParseHeader();
            ParseFactionWarfare();
            CleanUpHeaders();
            ParseInvolvedParties();

            if (!Result.Header.Timestamp.HasValue || Result.Header.FactionName.IsNullOrEmpty() || Result.Header.AllianceName.IsNullOrEmpty()
                || Result.Header.CorporationName.IsNullOrEmpty() || Result.Header.VictimName.IsNullOrEmpty() || Result.Header.ShipName.IsNullOrEmpty()
                || Result.Header.SystemName.IsNullOrEmpty()) {
                return;
            }

            Result.Authorized = !checkAuthorization;
        }

        private void ParseHeader() {
            // header section
            var involvedpos = _killmail.IndexOf("Involved parties:");

            if (involvedpos == -1) {
                AddError("Mail lacks Involved parties header.");
                return;
            }

            var header = _killmail.Substring(0, involvedpos);

            DateTime timestamp;

            if (DateTime.TryParse(header.Substring(0, 16), out timestamp)) {
                Result.Header.Timestamp = timestamp;
            }

            var headerParts = _killmail.Substring(0, involvedpos).Trim().Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
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

                    if (Result.Header.MoonName.InsensitiveCompare(_unkown) && moonFound) {
                        Result.Header.MoonName = tmp;
                        Result.Header.VictimName = tmp;
                    }
                    continue;
                }

                if (Match("Security: (.*)", part, out tmp)) {
                    Result.Header.SystemSecurity = decimal.Parse(tmp);
                    continue;
                }

                if (Match("Damage Taken: (.*)", part, out tmp)) {
                    Result.Header.DamageTaken = decimal.Parse(tmp);
                    continue;
                }

                if (Match("Moon: (.*)", part, out tmp)) {
                    Result.Header.VictimName = tmp;
                    moonFound = true;
                    continue;
                }
            }

            if (moonFound) {
                Result.Header.VictimName = Result.Header.MoonName;
            }
        }

        private void ParseFactionWarfare() {
            if (Result.Header.AllianceName.InsensitiveCompare(_none))
                Result.Header.AllianceName = Result.Header.FactionName;
        }

        /// <summary>
        /// Report the errors for the things that make sense.
        /// we need pilot names, corp names, ship types, and the system to be sure
        /// the rest aren't required but for completeness, you'd want them in :)
        /// </summary>
        private void CleanUpHeaders() {
            if (Result.Header.VictimName.InsensitiveCompare(_unkown)) {
                AddError("Victim has no name.");
                Result.Header.VictimName = "";
            }

            if (Result.Header.CorporationName.InsensitiveCompare(_unkown)) {
                AddError("Victim has no corp.");
                Result.Header.CorporationName = "";
            }

            if (Result.Header.ShipName.InsensitiveCompare(_unkown)) {
                AddError("Victim has no ship type.");
                Result.Header.ShipName = "";
            }

            if (Result.Header.SystemName.InsensitiveCompare(_unkown)) {
                AddError("Killmail lacks solar system information.");
                Result.Header.SystemName = "";
            }
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
            var involvedSection = _killmail.Substring(involvedPosition, end - involvedPosition).Trim();
            var involved = involvedSection.Split(new[] { "\n" }, StringSplitOptions.None);
            var groups = new Dictionary<int, List<string>>();
            var startPos = 0;

            for (var j = 0; j < involved.Length; j++) {

                var current = involved[j];
                var currentList = new List<string>();
                
                if (current != "" || involved[j + 1] == "")
                    continue;

                groups.Add(j, currentList);
                var k = startPos;

                while (k < j) {
                    currentList.Add(involved[k]);
                    k++;
                }

                startPos = j + 1;
            }

            var ipilot_count = 0; //allows us to be a bit more specific when errors strike
            var i = 0;
            var needs_final_blow = true;

            while (i < involved.Length) {
                var iparts = involved.Length;
                var finalblow = false;

                while (i < iparts) {
                    ipilot_count++;

                    var ipname = "Unknown";
                    var ianame = "None";
                    var ifname = "None";
                    var icname = "None";
                    var isname = "Unknown";
                    var iwname = "Unknown";
                    var idmgdone = "0";
                    var secstatus = "0.0";

                    while (involved[i] == "") {
                        //compensates for multiple blank lines between involved parties
                        i++;

                        if (i <= involved.Length)
                            continue;

                        AddError("Involved parties section prematurely ends.");
                        return;
                    }

                    for (var counter = i; counter <= iparts - 1; counter++) {
                        string tmp;
                        var current = involved[counter];

                        if (Match("Name: (.*)", current, out tmp)) {
                            var slash = current.IndexOf("/");
                            if (slash >= 0) {
                                var name = current.Substring(5, slash - 5).Trim();
                                var corporation = current.Substring(slash + 1, current.Length - slash + 1).Trim();

                                // now if the corp bit has final blow info, note it
                                if (corporation.IndexOf("laid the final blow") > 0) {
                                    finalblow = true;
                                    iwname = name;
                                    end = corporation.IndexOf("(") - 1;
                                    corporation = corporation.Substring(0, end);
                                } else {
                                    finalblow = false;
                                    iwname = name;
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
                                ipname = tmp;
                                string tmpipname;
                                if (Match(@"(.*) \(laid the final blow\)", ipname, out tmpipname)) {
                                    ipname = tmpipname;
                                    finalblow = true;
                                } else {
                                    finalblow = false;
                                }
                            }

                            continue;
                        }

                        if (Match("Alliance: (.*)", current, out tmp)) {
                            ianame = tmp;
                            continue;
                        }

                        if (Match("Faction: (.*)", current, out tmp)) {
                            ifname = tmp;
                            continue;
                        }

                        if (Match("Corp: (.*)", current, out tmp)) {
                            icname = tmp;
                            continue;
                        }

                        if (Match("Ship: (.*)", current, out tmp)) {
                            isname = tmp;
                            continue;
                        }

                        if (Match("Weapon: (.*)", current, out tmp)) {
                            iwname = tmp;
                            continue;
                        }

                        if (Match("Security: (.*)", current, out tmp)) {
                            secstatus = tmp;
                            continue;
                        }

                        if (Match("Damage Done: (.*)", current, out tmp)) {
                            idmgdone = tmp;
                            continue;
                        }

                        if (current == "") {
                            // allows us to process the involved party. This is the empty line after the
                            // involved party section
                            counter++;
                            i = counter;
                            break;
                        }

                        // skip over this entry, it could read anything, we don't care. Handy if/when
                        // new mail fields get added and we aren't handling them yet.
                        counter++;
                        i = counter;

                        if (!needs_final_blow)
                            continue;

                        finalblow = true;
                        needs_final_blow = false;
                    }

                    // Faction Warfare stuff
                    if (ianame.InsensitiveCompare(_none)) {
                        ianame = ifname;
                    }
                    // end faction warfare stuff

                    if (icname.InsensitiveCompare(_none)) {
                        AddError(string.Format("Involved party has no corp. (Party No. {0})", ipilot_count));
                    }

                    if (ipname.InsensitiveCompare(_unkown)) {
                        if (iwname.IndexOf("Mobile") >= 0 || iwname.IndexOf("Control Tower") >= 0) {
                            //for involved parties parsed that lack a pilot, but are actually POS or mobile warp disruptors
                            ipname = iwname;
                        } else {
                            AddError(string.Format("Involved party has no name. (Party No. {0})", ipilot_count));
                        }
                    }

                    if (iwname.InsensitiveCompare(_unkown)) {
                        AddError(string.Format("No weapon found for pilot {0}.", ipname));
                    }

                    var party = new KillInvolvedParty {
                        PilotName = ipname,
                        CorporationName = icname,
                        AllianceName = ianame,
                        SecurityStatus = decimal.Parse(secstatus),
                        ShipName = isname,
                        WeaponName = iwname,
                        DamageDone = decimal.Parse(idmgdone),
                        FinalBlow = finalblow
                    };

                    Result.AddInvolvedParty(party);
                }
            }
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