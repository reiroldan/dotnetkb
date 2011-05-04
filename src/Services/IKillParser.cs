namespace DotNetKillboard.Services
{
    public interface IKillMailParser
    {
        bool Parse(string killmail);
    }
}