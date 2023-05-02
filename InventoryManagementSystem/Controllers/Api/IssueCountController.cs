using InventoryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueCountController : ControllerBase
    {
        private readonly InventoryManagementContext _db;

        public IssueCountController(InventoryManagementContext db)
        {
            _db = db;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var PendingList = _db.Issue.Count(x => x.Isview == 0 && x.Action == "pending");
                var AcceptList = _db.Issue.Count(a => a.Action == "accept" && a.Isview==1);
                var RejectList = _db.Issue.Count(a => a.Action == "reject");
                var ResolvedList = _db.Issue.Count(a =>a.Action == "resolved");
                var TotalList = PendingList + AcceptList + RejectList + ResolvedList;
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Unread Issue", data = new { PendingList, AcceptList, RejectList, ResolvedList, TotalList }  });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }
    }
}
