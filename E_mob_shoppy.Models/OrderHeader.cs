using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; } 

        public string ApplicationUserId {  get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }    


        public DateTime OrderDate { get; set; }
        public DateTime shippingDate { get; set; }
        public double OrderTotal {  get; set; } 



        public string? OrderStatus {  get; set; }   
        public string? PaymentStatus {  get; set; } 
        public string? TrackingNumber {  get; set; } 
        public string? Carrier {  get; set; }   


        public DateTime PaymentDate { get; set; }   
        public DateTime PaymentDueDate {  get; set; }   


        public string? SessionId {  get; set; } 
        public string? PaymentIntentId {  get; set; }

        public string? AppliedCouponCode { get; set; }
        public double? DiscountAmount { get; set; }



        [Required]
        public string PhoneNumber {  get; set; }

        [Required]
        public string streetAddress {  get; set; }

        [Required]
        public string City {  get; set; }
        [Required]
        public string state { get; set; }

        [Required]
        public string postalCode { get; set; }
        [Required]
        public string Name { get; set; }
        

    }
}
