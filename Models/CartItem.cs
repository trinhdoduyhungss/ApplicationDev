using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ApplicationDev.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public virtual  Product Product { get; init; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreateAt { get; set; }
    }
}