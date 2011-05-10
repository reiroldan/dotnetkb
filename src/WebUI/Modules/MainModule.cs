using DotNetKillboard.Reporting;
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
        private readonly IReportingRepository _repository;

        public AllianceModule(IReportingRepository repository)
            : base(@"alliance/(?<id>[\d]+)") {

            _repository = repository;

            Get["/kills"] = x => {
                //var kills = _repository.QueryFor<IAllianceKillsQuery>(q => q.Id = x.id).Execute();
                return Request.Uri;
            };

            Get["/losses"] = x => {
                return Request.Uri;
            };

            Get["/corps"] = x => {
                return Request.Uri;
            };

            Get["/ships_weapons"] = x => {
                return Request.Uri;
            };

            Get["/systems"] = x => {
                return Request.Uri;
            };

            Get["/corp_kills"] = x => {
                return Request.Uri;
            };

            Get["/corp_loosses"] = x => {
                return Request.Uri;
            };

            Get["/corp_kills_class"] = x => {
                return Request.Uri;
            };

            Get["/corp_losses_class"] = x => {
                return Request.Uri;
            };

            Get["/pilot_kills"] = x => {
                return Request.Uri;
            };

            Get["/pilot_loosses"] = x => {
                return Request.Uri;
            };

            Get["/pilot_kills_class"] = x => {
                return Request.Uri;
            };

            Get["/pilot_losses_class"] = x => {
                return Request.Uri;
            };
        }
    }

    public class CorporationModule : NancyModule
    {
        public CorporationModule()
            : base(@"corp/(?<id>[\d]+)") {

            Get["/"] = x => {
                return 123;
            };
        }
    }

    public class PilotModule : NancyModule
    {
        public PilotModule()
            : base(@"pilot/(?<id>[\d]+)") {
        }
    }
}