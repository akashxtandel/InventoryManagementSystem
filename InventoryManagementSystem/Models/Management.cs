using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models
{
    public class Management
    {
        public Management()
        {
            Issue = new HashSet<Issue>();
            Notification = new HashSet<Notification>();
        }

        public int ManagementId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployEmailId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Designation { get; set; }
        public string AssignProjectName { get; set; }
        public DateTime DateOfIssue { get; set; }
        public int? Monitor { get; set; }
        public int? Cpu { get; set; }
        public int? Keybord { get; set; }
        public int? Mouse { get; set; }
        public int?  Headphone { get; set; }
        public int? Camera { get; set; }
        public string? Others { get; set; }
        public int IsDelete { get; set; }

        public virtual ICollection<Issue> Issue { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<OthersAppliance> OthersAppliances { get; set; }
    }
}
