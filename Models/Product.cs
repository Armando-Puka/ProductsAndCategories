#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ProductsAndCategories.Models;   
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product Name is required")]
    public string ProductName { get; set; } 

    [Required(ErrorMessage = "Product Description is required")]
    public string ProductDescription { get; set; }

    [Required(ErrorMessage = "Price is required")]
    public int Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Association> ProductAssociation { get; set; } = new List<Association>();
}