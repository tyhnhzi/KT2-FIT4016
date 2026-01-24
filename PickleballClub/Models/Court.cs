using System.ComponentModel.DataAnnotations;

namespace PickleballClub.Models
{
    public class Court
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
