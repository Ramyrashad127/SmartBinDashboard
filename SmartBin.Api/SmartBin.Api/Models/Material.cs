namespace SmartBin.Api.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BinSection> BinSections { get; set; }
    }
    public enum MaterialType
    {
        Plastic = 1,
        Metal = 2,
    }
}
