using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBin.Api.Models
{
    public class SurroundingWaste
    {
        public int Id { get; set; }
        public string WasteLevel { get; set; }
        public int DetectedObjectsCount { get; set; }
        public float AiConfidence { get; set; }
        public DateTime TimeStmp { get; set; }

        [ForeignKey("Bin")]
        public int BinId { get; set; }
        public Bin Bin { get; set; }

    }
}
