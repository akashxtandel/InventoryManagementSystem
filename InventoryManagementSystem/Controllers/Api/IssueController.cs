using InventoryManagementSystem.Data;
using InventoryManagementSystem.DTO;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace InventoryManagementSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly InventoryManagementContext _db;

        public IssueController(InventoryManagementContext db)
        {
            _db = db;
        }
        // all 
        public List<GetIssue> GetStatusList(string action) {

                var objlist = (from c in _db.Issue
                               join b in _db.Management
                               on c.ManagementId equals b.ManagementId

                               where
                               EF.Functions.Like(c.Action, "%" + action + "%") &&
                               EF.Functions.Like(c.IsDelete.ToString(), "0")
                               select new
                               {
                                   c.IssueId,
                                   b.ManagementId,
                                   b.EmployeeName,
                                   c.Appliances,
                                   c.Date,
                                   b.AssignProjectName,
                                   c.IssueDescription,
                                   c.Action,
                                   c.AdminResponse,
                                   c.ResolveTime,
                                   c.Isview
                               }).ToList().OrderByDescending(p => p.IssueId);
                List<GetIssue> getIssues = new List<GetIssue>();
                foreach (var item in objlist)
                {
                    var getIssue = new GetIssue();
                    var numbers = item.Appliances.Split(',').Select(Int32.Parse).ToList();
                    foreach (var Appliancesid in numbers)
                    {
                        var tempApp = new getApplinace();
                        var resolved = _db.Appliance.Where(x => x.ApplianceId == Appliancesid).FirstOrDefault();
                        getIssue.ApplianceCode.Add(Appliancesid.ToString());
                        getIssue.BrandName.Add(resolved.BrandId.ToString());
                    }
                    getIssue.IssueId = item.IssueId;
                    getIssue.Action = item.Action;
                    getIssue.managementId = item.ManagementId;
                    getIssue.EmployeeName = item.EmployeeName;
                    getIssue.Date = item.Date;
                    getIssue.IssueDescription = item.IssueDescription;
                    getIssue.AssignProjectName = item.AssignProjectName;
                    getIssue.AdminResponse = item.AdminResponse;
                    getIssue.ResolveTime = item.ResolveTime;                
                    getIssues.Add(getIssue);
                }
                return getIssues;
           //new change test
        }

        //count all 
        [Authorize]
        [HttpGet]
        [Route("Count_Issues")]
        public IActionResult Count_Issues()
        {
            try
            {
                var PendingList = _db.Issue.Count(x => x.Action == "pending" && x.IsDelete == 0);
                var AcceptList = _db.Issue.Count(a => a.Action == "accept" && a.IsDelete==0);
                var RejectList = _db.Issue.Count(a => a.Action == "reject" && a.IsDelete == 0);
                var ResolvedList = _db.Issue.Count(a => a.Action == "resolved" && a.IsDelete == 0);
                var TotalList = PendingList + AcceptList + RejectList + ResolvedList;
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Unread Issue", data = new { PendingList, AcceptList, RejectList, ResolvedList, TotalList } });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        //pending
        [Authorize(Roles = "Employee,Admin")]
        [HttpGet]
        [Route("PendingIssues")]
        public IActionResult GetPendingIssues()
        {
            try
            {
                List<GetIssue> getIssues = GetStatusList("pending");
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Pending Issues", data = getIssues });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        //Accept
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("AcceptedIssues")]
        public IActionResult GetAcceptIssues()
        {
            try
            {
                List<GetIssue> getIssues = GetStatusList("accept");
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Accepted Issues", data = getIssues });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }


        //Accept
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllIssues")]
        public IActionResult GetAllIssues()
        {
            try
            {
                List<GetIssue> getPendingIssues = GetStatusList("pending");
                List<GetIssue> getAcceptIssues = GetStatusList("accept");
                List<GetIssue> getRejectIssues = GetStatusList("reject");
                List<GetIssue> getResolvedIssues = GetStatusList("resolved");
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Accepted Issues", data = new { getPendingIssues, getAcceptIssues, getRejectIssues, getResolvedIssues } });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }


        // Reject
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("RejectedIssues")]
        public IActionResult GetRejectIssues()
        {
            try
            {
                List<GetIssue> getIssues = GetStatusList("reject");
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Rejected Issues", data = getIssues });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }

        //GetResolvedIssues
        [Authorize(Roles = "Employee,Admin")]
        [HttpGet]
        [Route("ResolvedIssues")]
        public IActionResult GetResolvedIssues()
        {
            try
            {
                List<GetIssue> getIssues = GetStatusList("resolved");
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Resolved Issues", data = getIssues });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        //UpdateYesIssues
        [Authorize(Roles = "Employee,Admin")]
        [HttpPost]
        [Route("UpdateYesIssues")]
        public IActionResult UpdateYesIssues(int id)
        {
            try
            {
                var entity = _db.Issue.Where(x => x.IssueId == id && x.IsDelete == 0).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    entity.Action = "Resolved";
                    _db.SaveChanges();
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = $"{entity.Action} " });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }
        //UpdateNoIssues
        [Authorize(Roles = "Employee,Admin")]
        [HttpPost]
        [Route("UpdateNoIssues")]
        public IActionResult UpdateNoIssues(int id)
        {
            try
            {
                var entity = _db.Issue.Where(x => x.IssueId == id && x.IsDelete == 0).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    entity.Action = "Pending";
                    _db.SaveChanges();
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = $"{entity.Action} " });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        // Create Issue
        [Authorize(Roles = "Employee")]
        [HttpPost]
        public ActionResult CreateIssue(IssueDTO issue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in issue.Appliances)
                    {
                        var entity = _db.Appliance.Where(x => x.ApplianceId == item).FirstOrDefault();
                        if (entity != null)
                        {
                            if (entity.IsDelete == 1)
                            {
                                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = entity.ApplianceCode + " Appliance is deleted" });
                            }
                        }
                        else
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.OK, message = item + " Appliances not found in DB" });
                        }
                    }
                    var result = new Issue
                    {
                        IssueId = issue.IssueId,
                        ManagementId = issue.ManagementId,
                        Appliances = string.Join(",", issue.Appliances),
                        Date = issue.Date,
                        //Date = DateTime.Now,
                        IssueDescription = issue.IssueDescription,
                        Action = issue.Action
                    };
                    _db.Issue.Add(result);
                    _db.SaveChanges();
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Issue successfully sent to admin." });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        //Update
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("UpdateAccept")]
        public ActionResult UpdateAccept([FromBody] IssueDTO issue, int id)
        {
            try
            {
                var entity = _db.Issue.Where(x => x.IssueId == id && x.IsDelete == 0).FirstOrDefault();
                if(entity == null)
                {
                    return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "IssueId not found." });
                }
                if (ModelState.IsValid)
                {
                    entity.Action = "Accept";
                    entity.Time_Date = issue.Time_Date;
                    //entity.AdminResponse = $"{issue.AdminResponse} {issue.ResolveTime}";
                    entity.AdminResponse = issue.AdminResponse;
                    entity.ResolveTime = issue.ResolveTime;
                    entity.Time_Only = issue.Time_Only;
                    _db.SaveChanges();
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = new { entity.AdminResponse,entity.Time_Date,entity.ResolveTime, entity.Time_Only } });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }

        //Update
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [Route("UpdatePending")]
        public ActionResult UpdatePending([FromBody] IssueDTO issue, int id)
        {
            try
            {
                var entity = _db.Issue.Where(x => x.IssueId == id && x.IsDelete == 0).FirstOrDefault(); ;
                if (ModelState.IsValid)
                {

                    entity.Action = "Pending";
                    //entity.AdminResponse = $"{issue.AdminResponse} {issue.ResolveTime}";
                    //entity.ResolveTime = issue.ResolveTime;
                    _db.SaveChanges();
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = $"{entity.Action}" });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Somthing wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }

        //UpdateReject
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("UpdateReject")]
        public ActionResult UpdateReject([FromBody] IssueDTO issueDTO, int id)
        {
            try
            {
                var entity = _db.Issue.Where(x => x.IssueId == id && x.IsDelete == 0).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    entity.Action = "Reject";
                    entity.AdminResponse = $"{issueDTO.AdminResponse} {issueDTO.ResolveTime}";
                    entity.ResolveTime = issueDTO.ResolveTime;
                    _db.SaveChanges();
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = $"{entity.AdminResponse}" });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }

        // UpdateResolved
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [Route("UpdateResolved")]
        public ActionResult UpdateResolved([FromBody] IssueDTO issueDTO, int id)
        {
            try
            {
                var entity = _db.Issue.Where(x => x.IssueId == id && x.IsDelete == 0).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    entity.Action = "Resolved";
                  //  entity.AdminResponse = $"{issueDTO.AdminResponse} {issueDTO.ResolveTime}";
                  //  entity.ResolveTime = issueDTO.ResolveTime;*
                    _db.SaveChanges();
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = $"{entity.Action} " });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }   
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("GetByIdIssue")]
        public ActionResult GetByIdIssue(int id)
        {
            try
            {
                var objlist = (from c in _db.Issue
                               join b in _db.Management
                               on c.ManagementId equals b.ManagementId
                               where
                               c.IssueId == id
                               select new
                               {
                                   c.IssueId,
                                   c.ManagementId,
                                   b.EmployeeName,
                                   c.Appliances,
                                   c.Date,
                                   b.AssignProjectName,
                                   c.IssueDescription,
                                   c.Isview,
                                   c.Action
                               }).ToList();
                List<GetIssue> getIssues = new List<GetIssue>();
                foreach (var item in objlist)
                {
                    var getIssue = new GetIssue();
                    var numbers = item.Appliances.Split(',').Select(Int32.Parse).ToList();
                    foreach (var Appliancesid in numbers)
                    {
                        var tempApp = new getApplinace();
                        var resolved = _db.Appliance.Where(x => x.ApplianceId == Appliancesid).FirstOrDefault();
                        getIssue.ApplianceCode.Add(Appliancesid.ToString());
                        getIssue.BrandName.Add(resolved.BrandId.ToString());
                    }
                    getIssue.IssueId = item.IssueId;
                    getIssue.managementId = item.ManagementId;
                    getIssue.EmployeeName = item.EmployeeName;
                    getIssue.Date = item.Date;
                    getIssue.IssueDescription = item.IssueDescription;
                    getIssue.AssignProjectName = item.AssignProjectName;

                    getIssues.Add(getIssue);
                }
                var issue = getIssues.Select(p => new { p.IssueId, p.managementId, p.EmployeeName, p.Date, p.ApplianceCode,p.BrandName, p.IssueDescription, p.AdminResponse, p.AssignProjectName });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Issue Detail", data = issue });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("IsView")]
        public ActionResult IsView(int id)
        {
            var entity = _db.Issue.Where(x => x.IssueId == id && x.IsDelete == 0).FirstOrDefault();
            if (ModelState.IsValid)
            {
                entity.Isview = 1;
                _db.SaveChanges();
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "You Read the Issue." });
            }
            return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Delete")]
        public ActionResult Delete(int id)
        {
            var entity = _db.Issue.Where(x => x.IssueId == id).FirstOrDefault();
            if (entity != null && entity.IsDelete==0)
            {
                entity.IsDelete = 1;
                
            }
            else
            {
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Allready deleted." });
            }
            _db.SaveChanges();
            return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Delete Successfully" });
            
        }


        [Authorize(Roles = "Employee")]
        [HttpGet]
        [Route("IssueStatus")]
        public ActionResult IssueStatus(int id)
        {
            var objlist = (from c in _db.Issue
                           join b in _db.Management
                           on c.ManagementId equals b.ManagementId
                           where
                          c.ManagementId == id && c.IsDelete==0
                           select new
                           {
                               c.IssueId,
                               b.ManagementId,
                               b.EmployeeName,
                               c.Appliances,
                               c.Date,
                               b.AssignProjectName,
                               c.IssueDescription,
                               c.AdminResponse,
                               c.ResolveTime,
                               c.Action,
                               c.Time_Date
                           }).OrderByDescending(p=>p.IssueId).ToList();
            List<GetIssue> getIssues = new List<GetIssue>();                                                                                     
            foreach (var item in objlist)
            {
                var getIssue = new GetIssue();
                var numbers = item.Appliances.Split(',').Select(Int32.Parse).ToList();
                foreach (var Appliancesid in numbers)
                {
                    var resolved = _db.Appliance.Where(x => x.ApplianceId == Appliancesid).FirstOrDefault();
                    var tempApp = new getApplinace();
                    if (resolved != null)
                    {
                        tempApp.BrandId = resolved.BrandId.ToString();

                        var brand = (from b in _db.BrandDetail
                                     where b.BrandId == resolved.BrandId
                                     select new
                                     {
                                         b.BrandName
                                     }).FirstOrDefault();
                        tempApp.BrandName = brand.BrandName;
                    }
                    tempApp.ApplianceCode = resolved.ApplianceCode;
                    getIssue.ApplianceCode.Add($"{ resolved.ApplianceCode}-{ tempApp.BrandName}");
                    //getIssue.ApplianceCode.Add($"{resolved.ApplianceCode}-{resolved.BrandName}");
                    //getIssue.BrandName.Add(resolved.BrandName);
                }
                getIssue.IssueId = item.IssueId;
                getIssue.managementId = item.ManagementId;
                getIssue.EmployeeName = item.EmployeeName;
                getIssue.Date = item.Date;
                getIssue.IssueDescription = item.IssueDescription;
                getIssue.AdminResponse = item.AdminResponse;
                getIssue.ResolveTime = item.ResolveTime;
                getIssue.AssignProjectName = item.AssignProjectName;
                getIssue.Action = item.Action;
                getIssue.AdminResponse = item.AdminResponse;
                getIssue.ResolveDate = item.Time_Date;

                getIssues.Add(getIssue);
            }
            var issue = getIssues.Select(p => new { 
                p.IssueId, p.managementId, 
                p.EmployeeName, p.Date, 
                p.ApplianceCode, p.IssueDescription, 
                p.AdminResponse, p.AssignProjectName,
                p.ResolveTime,p.Action,
                p.ResolveDate
            });
            return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Issue Status", data = issue });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("MultipleRejectDelete")]
        public ActionResult DeleteSelected(IEnumerable<int> id)
        {
            try
            {
                foreach (int ids in id)
                {
                    var Issue = _db.Issue.Where(x => x.IssueId == ids).FirstOrDefault();
                    if (Issue != null && Issue.Action=="Reject" && Issue.IsDelete==0)
                    {
                        Issue.IsDelete = 1;
                    }
                    else
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Allready deleted" });
                    }

                }
                _db.SaveChanges();
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Delete successfully" });
               

            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("MultipleResolvedDelete")]
        public ActionResult DeleteSelectedData(IEnumerable<int> id)
        {
            try
            {
                foreach (int ids in id)
                {
                    var Issue = _db.Issue.Where(x => x.IssueId == ids).FirstOrDefault();
                    if (Issue != null && Issue.Action == "Resolved" && Issue.IsDelete == 0)
                    {
                        Issue.IsDelete = 1;
                       
                    }
                    else
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Allready deleted" });
                    }

                }
                _db.SaveChanges();
                //return new JsonResult(new { status = (int)HttpStatusCode.OK, message = " Please Check Action Value" });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Delete successfully" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("MultiplePendingDelete")]
        public ActionResult DeletePendingSelected(IEnumerable<int> id)
        {
            try
            {
                foreach (int ids in id)
                {
                    var Issue = _db.Issue.Where(x => x.IssueId == ids).FirstOrDefault();
                    if (Issue != null && Issue.Action == "Pending" && Issue.IsDelete == 0)
                    {
                        Issue.IsDelete = 1;

                    }
                    else
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Allready deleted" });
                    }

                }
                _db.SaveChanges();
                //return new JsonResult(new { status = (int)HttpStatusCode.OK, message = " Please Check Action Value" });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Delete successfully" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("MultipleAcceptDelete")]
        public ActionResult DeleteAcceptSelected(IEnumerable<int> id)
        {
            try
            {
                foreach (int ids in id)
                {
                    var Issue = _db.Issue.Where(x => x.IssueId == ids).FirstOrDefault();
                    if (Issue != null && Issue.Action == "Accept" && Issue.IsDelete == 0)
                    {
                        Issue.IsDelete = 1;

                    }
                    else
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Allready deleted" });
                    }

                }
                _db.SaveChanges();
                //return new JsonResult(new { status = (int)HttpStatusCode.OK, message = " Please Check Action Value" });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Delete successfully" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }
    }
}
