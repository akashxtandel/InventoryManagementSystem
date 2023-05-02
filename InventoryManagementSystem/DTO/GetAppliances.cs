using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Http;

namespace InventoryManagementSystem.DTO
{
    public class GetAppliances
    {
        public int ApplianceId { get; set; }
        public int CategoryId { get; set; }
        public string ApplianceCode { get; set; }

        public List<IFormFile> ApplianceImage { get; set; } = new List<IFormFile>();
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

    }
}
