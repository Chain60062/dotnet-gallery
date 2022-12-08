using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gallery.Models
{
    public partial class Album
    {
        [Key]
        [Required]
        public int? AlbumId { get; set; }
        public string? AlbumName { get; set; }
        public string? Description { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public string UserId { get; set; } = null!;

        [InverseProperty("Albums")]
        public virtual IList<Image>? Images { get; set; }

    }

    public class AlbumViewModel
    {
        [Required]
        public string AlbumName { get; set; } = null!;
        public string? Description { get; set; }
        public virtual IList<Image>? Images { get; set; }
    }
}