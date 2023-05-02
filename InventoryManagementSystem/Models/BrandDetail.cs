using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models
{
    public class BrandDetail
    {
        [Key]
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int IsDelete{ get; set; }

        public virtual ICollection<Appliance> Appliance { get; set; }
    }
}
