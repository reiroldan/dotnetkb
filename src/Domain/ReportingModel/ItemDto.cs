namespace DotNetKillboard.ReportingModel
{
    public class ItemDto
    {
        public int Id { get; set; }
        
        public int GroupId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int Icon { get; set; }
        
        public decimal Radius { get; set; }
        
        public decimal Mass { get; set; }
        
        public decimal Volume { get; set; }
        
        public decimal Capacity { get; set; }
        
        public int PortionSize { get; set; }
        
        public int RaceId { get; set; }
        
        public decimal BasePrice { get; set; }
        
        public int MarketGroup { get; set; }
    }
}