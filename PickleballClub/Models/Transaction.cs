using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClub.Models
{
    [Table("248_Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Foreign key to category
        [ForeignKey("TransactionCategory")]
        public int CategoryId { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
    }
}
