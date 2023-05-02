using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class OthersAppliance
    {
        [Key]
        public int Other_id { get; set; }
        public int Appliance_id { get; set; }
        public int Management_id { get; set; }

        public virtual Appliance Appliance { get; set; }
        public virtual Management Management { get; set; }
    }
}
