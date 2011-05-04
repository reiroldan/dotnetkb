namespace DotNetKillboard.ReportingModel
{
    public class SolarSystemDto
    {
        public int Id { get; set; }

        public int ConstellationId { get; set; }
        
        public string Name { get; set; }
        
        public decimal X { get; set; }

        public decimal Y { get; set; }

        public decimal Z { get; set; }

        public decimal Security { get; set; }
    }
}