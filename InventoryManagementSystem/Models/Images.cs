using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models
{
    public class Images
    {
        [Key]
        public int ApplianceImageId { get; set; }
        public string ApplianceImage { get; set; }
       
        [ForeignKey("ApplianceId")]
        public int ApplianceID { get; set; }
        public Appliance Appliance { get; set; }
    }
}
