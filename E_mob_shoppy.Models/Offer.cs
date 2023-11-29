using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_mob_shoppy.Models
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }

        [Required]
        public string OfferName { get; set; }

        public string Offertype {  get; set; }
      
         public double OfferDiscount { get; set; }

        public string OfferDescription { get; set; }
    }
}
