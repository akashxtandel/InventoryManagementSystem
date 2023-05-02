using InventoryManagementSystem.Data;
using InventoryManagementSystem.DTO;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly InventoryManagementContext _db;
        private readonly JWTManagerRepository _jWTManager;

        public LoginController(InventoryManagementContext db, JWTManagerRepository jWTManager)
        {
            _db = db;
            _jWTManager = jWTManager;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] Management management)
        {
            try
            {
                var token = _jWTManager.Authenticate(management);
                if (token == null)
                {
                    return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Incorrect Email Or Password " });
                }

                var objList = _db.Management.Where(c => c.EmployEmailId == management.EmployEmailId).Select(p => new { p.ManagementId, p.EmployeeName, p.EmployEmailId, p.Role });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Login Successfully", data = new { Access_token = token, userdata = objList } });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }
    }
}
