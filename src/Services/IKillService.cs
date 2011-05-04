using DotNetKillboard.Services.Model;

namespace DotNetKillboard.Services
{
    public interface IKillService
    {
        void CreateKill(ParsedKillResult kill);
    }
}