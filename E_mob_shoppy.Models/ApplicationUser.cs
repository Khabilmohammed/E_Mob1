﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.Models
{
     public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name {  get; set; }   
        public string? StreetAddress {  get; set; } 
        public string? City { get; set; }   
        public string? State { get; set; }
        public string? PostalCode { get; set;}

        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        public double? wallet {  get; set; }    



    }
}
