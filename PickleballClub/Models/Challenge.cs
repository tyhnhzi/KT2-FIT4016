using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClub.Models
{
    public enum ChallengeType { Duel = 0, MiniGame = 1 }
    public enum ChallengeGameMode { None = 0, TeamBattle = 1, RoundRobin = 2 }
    public enum ChallengeStatus { Open = 0, Ongoing = 1, Finished = 2, Pending = 3 }

    [Table("248_Challenges")]
    public class Challenge
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public ChallengeType Type { get; set; } = ChallengeType.Duel;

        [Required]
        public ChallengeGameMode GameMode { get; set; } = ChallengeGameMode.None;

        [Required]
        public ChallengeStatus Status { get; set; } = ChallengeStatus.Open;

        public string? RewardDescription { get; set; }

        [Required]
        public decimal EntryFee { get; set; } = 0;

        [Required]
        public decimal PrizePool { get; set; } = 0;

        public int? Config_TargetWins { get; set; }
        public int CurrentScore_TeamA { get; set; } = 0;
        public int CurrentScore_TeamB { get; set; } = 0;

        public int? CreatedByMemberId { get; set; }
        [ForeignKey("CreatedByMemberId")]
        public Member? Creator { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Participant>? Participants { get; set; }
    }
}
