using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBin.Api.Models
{
    public class Bin
    {
        public int Id { get; set; }
        public string IdentificationToken { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public List<BinSection> BinSections { get; set; }
        public List<Transection> Transections { get; set; }
    }
}
