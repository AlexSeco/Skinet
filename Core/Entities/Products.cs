using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Products
{
    [Key]
    public int ProductId { get; set; }
    public string ProductName { get; set; }
}
