using InventoryManagementSystem.Data;
using InventoryManagementSystem.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly InventoryManagementContext _db;

        public SearchController(InventoryManagementContext db)
        {
            _db = db;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("SearchVendor")]
        public ActionResult SearchVendor(string searchString)
        {
            try
            {
                var Vendor = from c in _db.Vendor
                             where
                             EF.Functions.Like(c.SupplierName, "%" + searchString + "%") ||
                             EF.Functions.Like(c.SupplierNumber, "%" + searchString + "%")
                             select c;
                var Vendorlist = Vendor.AsEnumerable().Where(c => c.IsDelete == 0).OrderByDescending(p=>p.SupplierId).Select(p => new { p.SupplierId, p.SupplierName, p.SupplierNumber });
                if (Vendorlist == null)
                {
                    return new JsonResult(new
                    {
                        status = (int)HttpStatusCode.OK,
                        message = "No Search Detail"
                    });
                }
                return new JsonResult(new
                {
                    status = (int)HttpStatusCode.OK,
                    message = "No Search Detail",
                    vendor = Vendorlist
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("SearchAppliance")]
        public ActionResult SearchAppliance(string searchString)
        {
            try
            {
                var Appliance = (from c in _db.Appliance
                                join b in _db.Vendor
                                 on c.VendorId equals b.SupplierId
                                 join br in _db.BrandDetail
                                 on c.BrandId equals br.BrandId
                                where
                                EF.Functions.Like(c.ApplianceCode, "%" + searchString + "%") ||
                                EF.Functions.Like(br.BrandName, "%" + searchString + "%") ||
                                EF.Functions.Like(c.PruchaseDate.ToString(), "%" + searchString + "%") ||
                                EF.Functions.Like(c.WarrantPeriod, "%" + searchString + "%") ||
                                EF.Functions.Like(b.SupplierName, "%" + searchString + "%") ||
                                EF.Functions.Like(b.SupplierNumber, "%" + searchString + "%")
                                select new
                                {
                                    c.ApplianceId,
                                    c.ApplianceCode,
                                    br.BrandName,
                                    c.PruchaseDate,
                                    c.WarrantPeriod,
                                    c.VendorId,
                                    b.SupplierName,
                                    b.SupplierNumber

                                }).OrderByDescending(p=>p.ApplianceId);
                if (Appliance == null)
                {
                    return new JsonResult(new
                    {
                        status = (int)HttpStatusCode.OK,
                        message = "No Search Detail"

                    });
                }
                return new JsonResult(new
                {
                    status = (int)HttpStatusCode.OK,
                    message = "Search Detail",
                    Appliance = Appliance
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        //SearchEmployeeManagement
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("SearchEmployeeManagement")]
        public ActionResult SearchEmployeeManagement(string searchString)
        {
            try
            {
                var EmployeeManagement = (from c in _db.Management
                                         join b in _db.Appliance
                                          on c.Monitor equals b.ApplianceId
                                         where
                                EF.Functions.Like(c.EmployeeName, "%" + searchString + "%") ||
                                EF.Functions.Like(c.EmployEmailId, "%" + searchString + "%") ||
                                EF.Functions.Like(c.DateOfIssue.ToString(), "%" + searchString + "%") ||
                                EF.Functions.Like(c.AssignProjectName, "%" + searchString + "%") ||
                                EF.Functions.Like(c.Designation, "%" + searchString + "%") ||
                                EF.Functions.Like(c.Password, "%" + searchString + "%")
                                         select new
                                         {
                                             c.ManagementId,
                                             c.EmployeeName,
                                             c.EmployEmailId,
                                             c.Password,
                                             c.Designation,
                                             c.AssignProjectName,
                                             c.DateOfIssue
                                         }).OrderByDescending(p=>p.ManagementId);
                if (EmployeeManagement == null)
                {
                    return new JsonResult(new
                    {
                        status = (int)HttpStatusCode.OK,
                        message = "No Search Detail"

                    });
                }
                return new JsonResult(new
                {
                    status = (int)HttpStatusCode.OK,
                    message = "Search Detail",
                    EmployeeManagement = EmployeeManagement
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        //SearchPendingPage 

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("SearchPendingPage")]
        public ActionResult SearchPendingPage(string searchString)
        {
            try
            {
                var objlist = (from c in _db.Issue
                               join b in _db.Management
                               on c.ManagementId equals b.ManagementId
                               where
                               EF.Functions.Like(c.Action, "%" + "pending" + "%") &&
                               EF.Functions.Like(c.IsDelete.ToString(), "0")
                               select new
                               {
                                   c.IssueId,
                                   b.EmployeeName,
                                   c.Appliances,
                                   c.Date,
                                   b.AssignProjectName,
                                   c.IssueDescription,
                                   c.IsDelete
                               }).ToList().OrderByDescending(p => p.IssueId);
                List<GetIssue> GetIssues = new List<GetIssue>();
                foreach (var item in objlist)
                {
                    var getIssue = new GetIssue();
                    var numbers = item.Appliances.Split(',').Select(Int32.Parse).ToList();
                    foreach (var id in numbers)
                    {
                        var resolved = _db.Appliance.Where(x => x.ApplianceId == id).FirstOrDefault();
                        getIssue.ApplianceCode.Add(resolved.ApplianceCode);
                        if (resolved != null)
                        {
                            var brand = (from b in _db.BrandDetail
                                         where b.BrandId == resolved.BrandId
                                         select new
                                         {
                                             b.BrandName
                                         }).FirstOrDefault();
                            getIssue.BrandName.Add(brand.BrandName);
                        }
                    }
                    getIssue.IssueId = item.IssueId;
                    getIssue.EmployeeName = item.EmployeeName;
                    getIssue.Date = item.Date;
                    getIssue.IssueDescription = item.IssueDescription;
                    getIssue.AssignProjectName = item.AssignProjectName;
                    GetIssues.Add(getIssue);
                }
                var EmployeeManagement = (from c in GetIssues
                                          where
                                 EF.Functions.Like(c.EmployeeName, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.ApplianceCode.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.BrandName.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.Date.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.AssignProjectName, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.IssueDescription, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.ApplianceCode.ToString(), "%" + searchString + "%")

                                          select new
                                          {
                                              c.IssueId,
                                              c.EmployeeName,
                                              c.ApplianceCode,
                                              c.BrandName,
                                              c.Date,
                                              c.AssignProjectName,
                                              c.IssueDescription,
                                              c.IsDelete
                                          }).OrderByDescending(p => p.IssueId);
                EmployeeManagement.AsEnumerable().Where(c => c.IsDelete == 0).Select(p => new { p.EmployeeName, p.ApplianceCode, p.BrandName, p.Date, p.AssignProjectName, p.IssueDescription });
                if (EmployeeManagement == null)
                {
                    return new JsonResult(new
                    {
                        status = (int)HttpStatusCode.OK,
                        message = "No Search Detail"
                    });
                }

                return new JsonResult(new
                {
                    status = (int)HttpStatusCode.OK,
                    message = "Search Detail",
                    Pending = EmployeeManagement
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }


        //SearchAcceptPage
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("SearchAcceptPage")]
        public ActionResult SearchAcceptPage(string searchString)
        {
            try
            {
                var objlist = (from c in _db.Issue
                               join b in _db.Management
                               on c.ManagementId equals b.ManagementId
                               where
                               EF.Functions.Like(c.Action, "%" + "accept" + "%") &&
                               EF.Functions.Like(c.IsDelete.ToString(), "0")
                               select new
                               {
                                   c.IssueId,
                                   b.EmployeeName,
                                   c.Appliances,
                                   c.Date,
                                   b.AssignProjectName,
                                   c.IssueDescription,
                                   c.Action
                               }).ToList().OrderByDescending(p=>p.IssueId);
                List<GetIssue> GetIssues = new List<GetIssue>();
                foreach (var item in objlist)
                {
                    var getIssue = new GetIssue();
                    var numbers = item.Appliances.Split(',').Select(Int32.Parse).ToList();
                    foreach (var id in numbers)
                    {
                        var resolved = _db.Appliance.Where(x => x.ApplianceId == id).FirstOrDefault();
                        getIssue.ApplianceCode.Add(resolved.ApplianceCode);
                        if (resolved != null)
                        {
                            var brand = (from b in _db.BrandDetail
                                         where b.BrandId == resolved.BrandId
                                         select new
                                         {
                                             b.BrandName
                                         }).FirstOrDefault();
                            getIssue.BrandName.Add(brand.BrandName);
                        }
                    }
                    getIssue.IssueId = item.IssueId;
                    getIssue.EmployeeName = item.EmployeeName;
                    getIssue.Date = item.Date;
                    getIssue.IssueDescription = item.IssueDescription;
                    getIssue.AssignProjectName = item.AssignProjectName;
                    GetIssues.Add(getIssue);
                }
                var EmployeeManagement = (from c in GetIssues
                                          where
                                 EF.Functions.Like(c.EmployeeName, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.ApplianceCode.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.BrandName.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.Date.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.AssignProjectName, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.IssueDescription, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.ApplianceCode.ToString(), "%" + searchString + "%")

                                          select new
                                          {
                                              c.IssueId,
                                              c.EmployeeName,
                                              c.ApplianceCode,
                                              c.BrandName,
                                              c.Date,
                                              c.AssignProjectName,
                                              c.IssueDescription,
                                              c.IsDelete
                                          }).OrderByDescending(p => p.IssueId);
                EmployeeManagement.AsEnumerable().Where(c => c.IsDelete == 0).Select(p => new { p.EmployeeName, p.ApplianceCode, p.BrandName, p.Date, p.AssignProjectName, p.IssueDescription });
                if (EmployeeManagement == null)
                {
                    return new JsonResult(new
                    {
                        status = (int)HttpStatusCode.OK,
                        message = "No Search Detail"
                    });
                }

                return new JsonResult(new
                {
                    status = (int)HttpStatusCode.OK,
                    message = "Search Detail",
                    Accept = EmployeeManagement
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }


        //SearchRejectPage
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("SearchRejectPage")]
        public ActionResult SearchRejectPage(string searchString)
        {
            try
            {
                var objlist = (from c in _db.Issue
                               join b in _db.Management
                               on c.ManagementId equals b.ManagementId
                               where
                               EF.Functions.Like(c.Action, "%" + "reject" + "%") && EF.Functions.Like(c.IsDelete.ToString(), "0")
                               select new
                               {
                                   c.IssueId,
                                   b.EmployeeName,
                                   c.Appliances,
                                   c.Date,
                                   b.AssignProjectName,
                                   c.IssueDescription,
                                   c.Action
                               }).ToList().OrderByDescending(p=>p.IssueId);
                List<GetIssue> GetIssues = new List<GetIssue>();
                foreach (var item in objlist)
                {
                    var getIssue = new GetIssue();
                    var numbers = item.Appliances.Split(',').Select(Int32.Parse).ToList();
                    foreach (var id in numbers)
                    {
                        var resolved = _db.Appliance.Where(x => x.ApplianceId == id).FirstOrDefault();
                        getIssue.ApplianceCode.Add(resolved.ApplianceCode);
                        if (resolved != null)
                        {
                            var brand = (from b in _db.BrandDetail
                                         where b.BrandId == resolved.BrandId
                                         select new
                                         {
                                             b.BrandName
                                         }).FirstOrDefault();
                            getIssue.BrandName.Add(brand.BrandName);
                        }
                    }
                    getIssue.IssueId = item.IssueId;
                    getIssue.EmployeeName = item.EmployeeName;
                    getIssue.Date = item.Date;
                    getIssue.IssueDescription = item.IssueDescription;
                    getIssue.AssignProjectName = item.AssignProjectName;
                    GetIssues.Add(getIssue);
                }
                var EmployeeManagement = (from c in GetIssues
                                          where
                                 EF.Functions.Like(c.EmployeeName, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.ApplianceCode.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.BrandName.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.Date.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.AssignProjectName, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.IssueDescription, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.ApplianceCode.ToString(), "%" + searchString + "%")
                                          select new
                                          {
                                              c.IssueId,
                                              c.EmployeeName,
                                              c.ApplianceCode,
                                              c.BrandName,
                                              c.Date,
                                              c.AssignProjectName,
                                              c.IssueDescription,
                                              c.IsDelete
                                          }).OrderByDescending(p => p.IssueId);
                EmployeeManagement.AsEnumerable().Where(c => c.IsDelete == 0).Select(p => new { p.EmployeeName, p.ApplianceCode, p.BrandName, p.Date, p.AssignProjectName, p.IssueDescription });
                if (EmployeeManagement == null)
                {
                    return new JsonResult(new
                    {
                        status = (int)HttpStatusCode.OK,
                        message = "No Search Detail"
                       
                    });
                }
                return new JsonResult(new
                {
                    status = (int)HttpStatusCode.OK,
                    message = "Search Detail",
                    Reject = EmployeeManagement
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        //SearchResolvedPage
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("SearchResolvedPage")]
        public ActionResult SearchResolvedPage(string searchString)
        {
            try
            {
                var objlist = (from c in _db.Issue
                               join b in _db.Management
                               on c.ManagementId equals b.ManagementId
                               where
                               EF.Functions.Like(c.Action, "%" + "resolved" + "%") && EF.Functions.Like(c.IsDelete.ToString(), "0")
                               select new
                               {
                                   c.IssueId,
                                   c.ManagementId,
                                   b.EmployeeName,
                                   c.Appliances,
                                   c.Date,
                                   b.AssignProjectName,
                                   c.IssueDescription,
                                   c.Action
                               }).ToList().OrderByDescending(p=>p.IssueId);
                List<GetIssue> GetIssues = new List<GetIssue>();
                foreach (var item in objlist)
                {
                    var getIssue = new GetIssue();
                    var numbers = item.Appliances.Split(',').Select(Int32.Parse).ToList();
                    foreach (var id in numbers)
                    {
                        var resolved = _db.Appliance.Where(x => x.ApplianceId == id).FirstOrDefault();
                        getIssue.ApplianceCode.Add(resolved.ApplianceCode);
                        if (resolved != null)
                        {
                            var brand = (from b in _db.BrandDetail
                                         where b.BrandId == resolved.BrandId
                                         select new
                                         {
                                             b.BrandName
                                         }).FirstOrDefault();
                            getIssue.BrandName.Add(brand.BrandName);
                        }
                    }
                    getIssue.IssueId = item.IssueId;
                    getIssue.EmployeeName = item.EmployeeName;
                    getIssue.Date = item.Date;
                    getIssue.IssueDescription = item.IssueDescription;
                    getIssue.AssignProjectName = item.AssignProjectName;
                    GetIssues.Add(getIssue);
                }
                var EmployeeManagement = (from c in GetIssues
                                          where
                                 EF.Functions.Like(c.EmployeeName, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.ApplianceCode.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.BrandName.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.Date.ToString(), "%" + searchString + "%") ||
                                 EF.Functions.Like(c.AssignProjectName, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.IssueDescription, "%" + searchString + "%") ||
                                 EF.Functions.Like(c.ApplianceCode.ToString(), "%" + searchString + "%")
                                          select new
                                          {
                                              c.IssueId,
                                              c.EmployeeName,
                                              c.ApplianceCode,
                                              c.BrandName,
                                              c.Date,
                                              c.AssignProjectName,
                                              c.IssueDescription,
                                              c.IsDelete
                                          }).OrderByDescending(p => p.IssueId);
                EmployeeManagement.AsEnumerable().Where(c => c.IsDelete == 0).Select(p => new { p.EmployeeName, p.ApplianceCode, p.BrandName, p.Date, p.AssignProjectName, p.IssueDescription });
                if (EmployeeManagement == null)
                    return new JsonResult(new
                    {
                        status = (int)HttpStatusCode.OK,
                        message = "No Search Detail"

                    });
                return new JsonResult(new
                {
                    status = (int)HttpStatusCode.OK,
                    message = "Search Detail",
                    Resolved = EmployeeManagement
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }
    }
}
