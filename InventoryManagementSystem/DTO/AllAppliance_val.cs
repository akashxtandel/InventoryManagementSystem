using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DTO
{
    public class ApplianceImages
    {
        public int ApplianceImageId { get; set; }
        public string ApplianceImage { get; set; }
    }
    public class AllAppliance_val
    {
        public int ApplianceId { get; set; }
        public string ApplianceName { get; set; }
        public int CategoryId { get; set; }
        public string ApplianceCode { get; set; }

        public List<ApplianceImages> ApplianceImage { get; set; } = new List<ApplianceImages>();
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public DateTime PurchaseDate { get; set; }

        public string WarrantPeriod { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNumber { get; set; }
        public int SupplierId { get; internal set; }
    }

}