﻿using System.ComponentModel.DataAnnotations;

namespace ApplicationDev.Models
{
    public class CheckoutDetails
    {
        [Key]
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public int Quantity { get; set; }
        
    }
}