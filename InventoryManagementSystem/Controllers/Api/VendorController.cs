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
    public class VendorController : ControllerBase
    {
        private readonly InventoryManagementContext _db;

        public VendorController(InventoryManagementContext db)
        {
            _db = db;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetVendor()
        {
            try
            {
                var objList = _db.Vendor.Where(x => x.IsDelete == 0).OrderByDescending(p=>p.SupplierId).Select(p => new { p.SupplierId, p.SupplierName, p.SupplierNumber,p.SupplierType,p.SupplierAddress });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Vendor Detail", data = objList });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message});
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateVendor(Vendor vendor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var old_vendor = _db.Vendor.Where(x => x.SupplierNumber == vendor.SupplierNumber).FirstOrDefault();
                    if (old_vendor == null) { 
                        _db.Vendor.Add(vendor);
                        _db.SaveChanges();
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record insert successfully" });
                    }
                    else
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "SupplierNumber already exited" });
                    }
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetByID")]
        public IActionResult GetByID(int id)
        {
            try
            {
                if (_db.Vendor.Any(c => c.SupplierId == id && c.IsDelete == 0))
                {
                    var objList = _db.Vendor.Where(x => x.IsDelete == 0 && x.SupplierId == id).OrderByDescending(p=>p.SupplierId).Select(p => new { p.SupplierId, p.SupplierName, p.SupplierNumber,p.SupplierType,p.SupplierAddress });
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Vendor Detail", data = objList });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.NotFound, message = "No Such Detail" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Update")]
        public ActionResult UpdateSupplier([FromBody] Vendor vendor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_db.Vendor.Where(c => c.IsDelete == 0).Any(x => x.SupplierId == vendor.SupplierId))
                    {
                        _db.Vendor.Update(vendor);
                        _db.SaveChanges();

                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Update successfully" });
                    }
                    return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Enater Valid ID" });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Somthing wrong." }); ;
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.InnerException.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Delete")]
        public ActionResult SoftDeleteBrand(int id)
        {
            try
            {
                var entity = _db.Vendor.Where(x => x.SupplierId == id).FirstOrDefault();
                if(_db.Appliance.Any(x => x.VendorId == entity.SupplierId))
                {
                    return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Vendors is assigned, Cannot delete this item." });
                }
                if (entity!=null && entity.IsDelete==0)
                {
                    entity.IsDelete = 1;
                }
                else
                {
                    return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Allready deleted" });
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
        [Route("MultipleDelete")]

        public ActionResult DeleteSelected(IEnumerable<int> id)
        {
            try
            {
                foreach (int ids in id)
                {
                    var entity = _db.Vendor.Where(x => x.SupplierId == ids).FirstOrDefault();
                    if (_db.Appliance.Any(x => x.VendorId == entity.SupplierId))
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = entity.SupplierNumber + " You can't remove vendor as it is assigned to appliances" });
                    }
                    if (entity != null && entity.IsDelete == 0)
                    {
                        entity.IsDelete = 1;
                    }
                    else
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Already deleted" });
                    }
                }
                _db.SaveChanges();
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Delete successfully" });
                //return new JsonResult(new { status = (int)HttpStatusCode.NotFound, message = "No Such Detail" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }
    }
}
