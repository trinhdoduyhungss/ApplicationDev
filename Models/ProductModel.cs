using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApplicationDev.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public int Pages { get; set; }
        public decimal Price { get; set; }
        public string? Desc { get; set; }
        public string? ImgUrl { get; set; }
        public  IEnumerable<SelectListItem>? ProductCategoryList { get; set; }
        [NotMapped]
        public  IEnumerable<SelectListItem>? StoreList { get; set; }
    }
}