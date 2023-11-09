using System.ComponentModel.DataAnnotations;

namespace ManeroBackend.Models
{
    public class CategorySchema
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
