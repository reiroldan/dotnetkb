using Nancy;

namespace DotNetKillboard.Modules
{
    public class MainModule : NancyModule
    {
        public MainModule() {
            Get["/"] = x => {
                return "Main";
            };
        }
    }

    public class AllianceModule : NancyModule
    {
        public AllianceModule()
            : base("alliance/{id}") {
        }
    }

    public class CorporationModule : NancyModule
    {

        public CorporationModule()
            : base("corp/{id}") {

        }
    }

    public class PilotModule : NancyModule
    {
        public PilotModule()
            : base("pilot") {
           
        }
    }
}