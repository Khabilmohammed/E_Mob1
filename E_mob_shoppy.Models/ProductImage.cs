using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.Models
{
    public class ProductImage
    {
        [Key]
        public int productImageId {  get; set; }
        [Required]
        public string ImageUrl {  get; set; }


        public int ProductId {  get; set; } 
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
