using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DTO
{
    public class GetIssue
    {
        public int IssueId { get; set; }
        public int managementId { get; set; }
        public string EmployeeName { get; set; }
        [JsonIgnore]
        public string Appliance { get; set; }
        public List<string> ApplianceCode { get; set; } = new List<string>();
        public List<string> BrandName { get; set; } = new List<string>();
        public string Action { get; set; }
        public int IsView { get; set; }
        public DateTime Date { get; set; }
        public string AssignProjectName { get; set; }
        public string IssueDescription { get; set; }
        public string AdminResponse { get; set; }
        public string ResolveTime { get; set; }
        public int IsDelete { get; set; }
        public DateTime? ResolveDate { get; set; }
    }


    public class GetPendingIssue
    {
        public int IssueId { get; set; }
        public int managementId { get; set; }
        public string EmployeeName { get; set; }
        public List<getApplinace> ApplianceList { get; set; } = new List<getApplinace>();
        public string Action { get; set; }
        public int IsView { get; set; }
        public DateTime Date { get; set; }
        public string AssignProjectName { get; set; }
        public string IssueDescription { get; set; }
        public string AdminResponse { get; set; }
    }

    public class getApplinace
    {
        public string? ApplianceCode { get; set; }
        public string? BrandName { get; set; }
        public string? BrandId { get; set; }

    }

    public class AllPendingIssue
    {
        public int IssueId { get; set; }
        public int managementId { get; set; }
        public string EmployeeName { get; set; }
        public string ApplianceCode { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public string Action { get; set; }
        public int IsView { get; set; }
        public DateTime Date { get; set; }
        public string AssignProjectName { get; set; }
        public string IssueDescription { get; set; }
        public string AdminResponse { get; set; }
    }

}
