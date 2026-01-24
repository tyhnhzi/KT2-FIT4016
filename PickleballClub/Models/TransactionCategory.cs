using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClub.Models
{
    public enum TransactionType
    {
        Income,
        Expense
    }

    [Table("248_TransactionCategories")]
    public class TransactionCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
