using System.Collections.Generic;
using DotNetKillboard.ReportingModel;

namespace DotNetKillboard.ReportingQueries
{
    public interface ISolarSystemByNameQuery : ISingleQuery<SolarSystemDto>
    {
        string Name { get; set; }
    }

    public interface IItemByNameQuery : ISingleQuery<ItemDto>
    {
        string Name { get; set; }
    }

    public interface IItemsWithNamesQuery : IListQuery<ItemDto>
    {
        IEnumerable<string> Names { get; set; }
    }
}