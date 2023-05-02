using InventoryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem
{
    public class Vendor
    {
        public Vendor()
        {
            Appliance = new HashSet<Appliance>();
        }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNumber { get; set; }
        public string SupplierType { get; set; }
        public string SupplierAddress { get; set; }
        public int IsDelete { get; set; }

        public virtual ICollection<Appliance> Appliance { get; set; }
    }
}
