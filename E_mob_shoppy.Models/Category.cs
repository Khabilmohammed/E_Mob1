using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_mob_shoppy.Models
{
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Display Order")]
        [Range(0,100,ErrorMessage ="Dispaly order must be 0 to 100")]
        public int DisplayOrder { get; set; }

    }
}
