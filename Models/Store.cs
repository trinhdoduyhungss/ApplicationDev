using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApplicationDev.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Product>? Products { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        [NotMapped]
        public  IEnumerable<SelectListItem>? UserList { get; set; }
        
    }
}