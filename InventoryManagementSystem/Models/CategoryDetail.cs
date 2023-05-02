using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class CategoryDetail
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public bool IsOthers { get; set; }

        public virtual ICollection<Appliance> Appliance { get; set; }
    }
}
