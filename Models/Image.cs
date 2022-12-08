using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gallery.Models
{
    public partial class Image
    {
        public Image()
        {
            Albums = new HashSet<Album>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ImageId { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Picture { get; set; } = null!;
        [NotMapped]
        public IFormFile? PictureFile { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public string UserId { get; set; } = null!;
        [Column(TypeName = "datetime2")]
        public DateTime? AddedAt { get; set; }

        [InverseProperty("Images")]
        public virtual ICollection<Album>? Albums { get; set; }
    }

    public class ImageViewModel
    {
        [Required]
        public IFormFile PictureFile { get; set; } = null!;
        public string? Picture { get; set; }
        public DateTime? AddedAt { get; set; }
    }
}