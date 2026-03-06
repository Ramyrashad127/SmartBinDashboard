using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBin.Api.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime TimeStmp { get; set; }
        public float? AiConfidence { get; set; }
        [ForeignKey("Bin")]
        public int BinId { get; set; }
        public Bin Bin { get; set; }
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }
    }
}
