using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClub.Models
{
    public enum ParticipantTeam { None = 0, TeamA = 1, TeamB = 2 }
    public enum ParticipantStatus { Pending = 0, Confirmed = 1, Withdrawn = 2 }

    [Table("248_Participants")]
    public class Participant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ChallengeId { get; set; }
        [ForeignKey("ChallengeId")]
        public Challenge? Challenge { get; set; }

        [Required]
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member? Member { get; set; }

        [Required]
        public ParticipantTeam Team { get; set; } = ParticipantTeam.None;

        public bool EntryFeePaid { get; set; } = false;
        public decimal EntryFeeAmount { get; set; } = 0;

        public DateTime JoinedDate { get; set; } = DateTime.Now;

        [Required]
        public ParticipantStatus Status { get; set; } = ParticipantStatus.Pending;
    }
}
