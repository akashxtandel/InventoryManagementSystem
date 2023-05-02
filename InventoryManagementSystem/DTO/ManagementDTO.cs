using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.DTO
{
    public class ManagementDTO
    {
        public int ManagementId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployEmailId { get; set; }
        public string Role { get; set; }
        public string BrandName { get; set; }
        public string Password { get; set; }
        public string Designation { get; set; }
        public string AssignProjectName { get; set; }
        public DateTime DateOfIssue { get; set; }
        public int? MonitorId { get; set; } 
        public int? CpuId { get; set; }

        public int? KeybordId { get; set; }
 
        public int? MouseId { get; set; }

        public int? HeadphoneId { get; set; }
  
        public int? CameraId { get; set; }
        public List<int>? OthersId { get; set; }

        public virtual BrandDetail BrandDetail { get; set; }

        public List<OthersAppliance> Other_Applinces { get; set; } = new List<OthersAppliance>();

    }
}
