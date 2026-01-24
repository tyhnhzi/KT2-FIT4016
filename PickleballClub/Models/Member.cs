using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PickleballClub.Models
{
    [Table("248_Members")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        public string IdentityUserId { get; set; }
        
        [ForeignKey("IdentityUserId")]
        public IdentityUser? User { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.Now;

        public double RankLevel { get; set; } = 1.0;

        public string Status { get; set; } = "Active"; // Active/Inactive

        public int TotalMatches { get; set; } = 0;
        public int WinMatches { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
