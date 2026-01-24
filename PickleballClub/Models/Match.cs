using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClub.Models
{
    public enum MatchFormat { Singles = 0, Doubles = 1 }
    public enum WinningSide { None = 0, Team1 = 1, Team2 = 2 }

    [Table("248_Matches")]
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime MatchDate { get; set; } = DateTime.Now;

        [Required]
        public MatchFormat Format { get; set; } = MatchFormat.Singles;

        public bool IsRanked { get; set; } = true;

        public int? ChallengeId { get; set; }
        [ForeignKey("ChallengeId")]
        public Challenge? Challenge { get; set; }

        // TEAM 1
        [Required]
        public int Team1_Player1Id { get; set; }
        [ForeignKey("Team1_Player1Id")]
        public Member? Team1_Player1 { get; set; }

        public int? Team1_Player2Id { get; set; }
        [ForeignKey("Team1_Player2Id")]
        public Member? Team1_Player2 { get; set; }

        // TEAM 2
        [Required]
        public int Team2_Player1Id { get; set; }
        [ForeignKey("Team2_Player1Id")]
        public Member? Team2_Player1 { get; set; }

        public int? Team2_Player2Id { get; set; }
        [ForeignKey("Team2_Player2Id")]
        public Member? Team2_Player2 { get; set; }

        [Required]
        public WinningSide WinningSide { get; set; } = WinningSide.None;

        public bool IsApproved { get; set; } = true;
    }
}
