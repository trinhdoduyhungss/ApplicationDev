using System.ComponentModel.DataAnnotations;

namespace ApplicationDev.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public virtual  Product? Product { get; init; }
        public DateTime CreateAt { get; set; }
    }
}