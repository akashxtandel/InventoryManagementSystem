using InventoryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DTO
{
    public class IssueDTO
    {
        public int IssueId { get; set; }
        public int ManagementId { get; set; }

        public List<int> Appliances { get; set; }
        public DateTime Date { get; set; }
        public string IssueDescription { get; set; }
        public string AdminResponse { get; set; }
        public string ResolveTime { get; set; }
        public string Action { get; set; } = "Pending";
        public int? Isview { get; set; }
        public int? IsDelete { get; set; }
        public DateTime? Time_Date { get; set; }
        public DateTime? Time_Only { get; set; }

        public virtual Management Management { get; set; }
    }
}
