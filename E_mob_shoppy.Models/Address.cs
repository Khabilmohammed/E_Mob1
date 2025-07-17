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
    public class Address
    {
        
           public int Id { get; set; }

           
            public string ApplicationUserId { get; set; }

            [ForeignKey("ApplicationUserId")]
            [ValidateNever]
            public ApplicationUser ApplicationUser { get; set; }

            [Required]
            public string StreetAddress { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string State { get; set; }

            [Required]
            public string PostalCode { get; set; }

           public bool? IsDefault { get; set; }
    

    }
}
