using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Net;
using InventoryManagementSystem.Migrations;

namespace InventoryManagementSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryDetailsController : ControllerBase
    {
        private readonly InventoryManagementContext _context;

        public CategoryDetailsController(InventoryManagementContext context)
        {
            _context = context;
        }

        // GET: api/CategoryDetails
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetCategoryDetail()
        {
            try
            {
                var objList = _context.CategoryDetail.OrderBy(p => p.CategoryId).Select(p => new { p.CategoryId, p.CategoryName,p.IsOthers });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Category Detail", data = objList });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        // GET: api/CategoryDetails/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public ActionResult GetCategoryDetailById(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var objList = _context.CategoryDetail.Where(x => x.CategoryId == id).OrderByDescending(p => p.CategoryId).Select(p => new { p.CategoryId, p.CategoryName });
                    //if (objList == null)
                    //{
                    //    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Id not found" });

                    //}
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Category Details id:", data = objList });

                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });

            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }
        // PUT: api/CategoryDetails/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Update")]

        public ActionResult UpdateCategoryDetail([FromBody] CategoryDetail categoryDetail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_context.CategoryDetail.Any(b => b.CategoryName == categoryDetail.CategoryName))
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "category Name Already Exist" });
                    }
                    else
                    {
                       _context.CategoryDetail.Update(categoryDetail);
                       _context.SaveChanges();
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Update successfully" });
                    }
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." }); ;
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        // POST: api/CategoryDetails
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateApplianceName([FromBody]CategoryDetail category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var others = _context.CategoryDetail.Where(x => x.CategoryName == category.CategoryName).ToList();
                    if(others.Count()!=0) {  
                         return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "CategoryName Already" });
                    }
                    _context.CategoryDetail.Add(category);
                    _context.SaveChanges();
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record insert successfully" });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }   
        }

        // DELETE: api/CategoryDetails/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryDetail(int id)
        {
            var categoryDetail = await _context.CategoryDetail.FindAsync(id);
            if (categoryDetail == null)
            {
                return NotFound();
            } 
            var app = _context.Appliance.Where(x => x.CategoryId == categoryDetail.CategoryId ).FirstOrDefault();
            if (app != null)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliances is assigned, Cannot delete this item." });
            }
            var app1 = _context.CategoryDetail.Where(x => x.CategoryId == categoryDetail.CategoryId && categoryDetail.IsOthers == false).FirstOrDefault();
            if (app1 != null)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Cannot delete this item." });
            }

            _context.CategoryDetail.Remove(categoryDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryDetailExists(int id)
        {
            return _context.CategoryDetail.Any(e => e.CategoryId == id);
        }
    }
}
