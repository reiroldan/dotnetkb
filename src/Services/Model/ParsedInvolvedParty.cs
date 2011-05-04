namespace DotNetKillboard.Services.Model
{
    public class ParsedInvolvedParty
    {
        public string PilotName { get; set; }

        public string CorporationName { get; set; }

        public string AllianceName { get; set; }

        public decimal SecurityStatus { get; set; }

        public string ShipName { get; set; }

        public string WeaponName { get; set; }

        public decimal DamageDone { get; set; }

        public bool FinalBlow { get; set; }

        public string FactionName { get; set; }
    }
}