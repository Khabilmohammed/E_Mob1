using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }

        [Required]
        public string Code { get; set; }

        public double? DiscountAmount { get; set; }

        public double? MinPurchaseAmount { get; set; }
        public double? MaxPurchaseAmount { get; set; }

        public DateTime StartDateTime { get; set; } = DateTime.Now;

        public DateTime ExpiryDateTime { get; set; }
    }
}
