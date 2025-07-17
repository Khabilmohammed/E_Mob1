using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.Models
{
    public class WalletHistory
    {
        [Key]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public double Amount { get; set; }

        public string TransactionType { get; set; } // e.g., "Credit", "Debit"

        public string Description { get; set; }
    }
}
