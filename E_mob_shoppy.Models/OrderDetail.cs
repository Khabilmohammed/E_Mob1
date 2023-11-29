using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.Models
{
    public class OrderDetail
    {

        [Key]
        public int OrderId {  get; set; }

        [Required]
        public int OrderHeaderId {  get; set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }    

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }    

        public int Count {  get; set; } 
        public double Price {  get; set; }  




    }
}
