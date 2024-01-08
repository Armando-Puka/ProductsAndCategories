#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ProductsAndCategories.Models;   
public class Category
{
    [Key]
    public int CategoryId { get; set; }
    
    [Required(ErrorMessage = "Category Name is required")]
    public string CategoryName { get; set; } 
   
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Association> CategoryAssociation { get; set; } = new List<Association>();
}