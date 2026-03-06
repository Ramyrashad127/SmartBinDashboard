using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBin.Api.Models
{
    public class BinSection
    {
        public int Id { get; set; }
        public float LevelPercentage { get; set; }
        public float Weight { get; set; }
        public DateTime LastUpdated { get; set; }

        [ForeignKey("Bin")]
        public int BinId { get; set; }
        public Bin Bin { get; set; }

        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }
    }
}
