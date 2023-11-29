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
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        
        [Required]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        
        [Required]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "List price")]
        public double ListPrice { get; set; } 

        [Required]
        [Display(Name ="List price 1-50")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "List price 50+")]
        public double Listprice50 { get; set; }

        [Required]
        [Display(Name = "List price 100+")]
        public double Listprice100 { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Quantity")]
        public int ProductQuantity { get; set; }

        public int Category_Id {  get; set; }
        [ForeignKey("Category_Id")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }



      




    }
}
