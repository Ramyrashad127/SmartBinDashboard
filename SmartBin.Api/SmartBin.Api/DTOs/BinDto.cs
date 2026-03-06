namespace SmartBin.Api.DTOs
{
    public class BinDto
    {
        public int Id { get; set; }
        public string IdentificationToken { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<BinSectionDto> Sections { get; set; } = new List<BinSectionDto>();
    }

    public class BinSectionDto
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public float LevelPercentage { get; set; }
        public float Weight { get; set; }
    }

    public class UpdateLocationDto
    {
        public string Token { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
