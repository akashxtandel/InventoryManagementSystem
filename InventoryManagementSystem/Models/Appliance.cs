using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models
{
    public class Appliance
    {
        

        public int ApplianceId { get; set; }
        public int CategoryId { get; set; }
        public string ApplianceCode { get; set; }
        public string ApplianceImage { get; set; }
       
        public int BrandId { get; set; }

        public DateTime PruchaseDate { get; set; }

        public string WarrantPeriod { get; set; }
        public int VendorId { get; set; } 
        public int IsActive { get; set; }
        public int IsAssign { get; set; }
        public int IsDelete { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual BrandDetail Brand { get; set; }
        public virtual CategoryDetail ApplianceCategory { get; set; }

        public virtual ICollection<Images> Images { get; set; }
        public virtual ICollection<OthersAppliance> OthersAppliances { get; set; }

    }
}
