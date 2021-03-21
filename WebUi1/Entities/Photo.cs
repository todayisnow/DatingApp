using System.ComponentModel.DataAnnotations.Schema;

namespace WebUi1.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int AppUserId { get; set; }
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
       // public AppUser AppUser { get; set; }
        
    }
}