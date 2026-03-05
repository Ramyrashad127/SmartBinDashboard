namespace SmartBin.Api.Models
{
    public class CrowdDensity
    {
        public int Id { get; set; }
        public string DensityLevel { get; set; }
        public int PeopleCount { get; set; }
        public float AiConfidence { get; set; }
        public DateTime TimeStmp { get; set; }
        public int BinId { get; set; }
        public Bin Bin { get; set; }
    }
}
