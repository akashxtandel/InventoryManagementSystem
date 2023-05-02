using InventoryManagementSystem.Data;
using InventoryManagementSystem.DTO;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

namespace InventoryManagementSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplianceController : ControllerBase
    {
        private readonly InventoryManagementContext _db;

        public ApplianceController(InventoryManagementContext db)
        {
            _db = db;
        }
        //Api/Appliance

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var objList = (from c in _db.Appliance
                               join b in _db.Vendor
                                on c.VendorId equals b.SupplierId
                                join br in _db.BrandDetail
                                on c.BrandId equals br.BrandId
                               join cat in _db.CategoryDetail
                              on c.CategoryId equals cat.CategoryId
                               where
                            c.IsDelete == 0
                               select new AllAppliance_val
                               {
                                   ApplianceId = c.ApplianceId,
                                   ApplianceCode = c.ApplianceCode,
                                   ApplianceName = cat.CategoryName,
                                   BrandName = br.BrandName,
                                   BrandId = br.BrandId,
                                   PurchaseDate = c.PruchaseDate,
                                   WarrantPeriod = c.WarrantPeriod,
                                   SupplierName = b.SupplierName,
                                   SupplierNumber = b.SupplierNumber
                               }).OrderByDescending(p => p.ApplianceId).ToList();

                foreach (var Appliance in objList)
                {
                    var img_obj = (from d in _db.Images
                                   where d.ApplianceID == Appliance.ApplianceId
                                   select new ApplianceImages
                                   {
                                       ApplianceImage = d.ApplianceImage,
                                       ApplianceImageId = d.ApplianceImageId
                                   }).OrderByDescending(p => p.ApplianceImageId).ToList();
                    
                    if (img_obj != null && img_obj.Count() > 0)
                    {
                        foreach (var image in img_obj)
                        {
                            var imagetest = new ApplianceImages();

                            imagetest.ApplianceImageId = image.ApplianceImageId;
                            imagetest.ApplianceImage = Request.Scheme.ToString() + "://" + Request.Host.ToString() + image.ApplianceImage;
                            Appliance.ApplianceImage.Add(imagetest);
                        }
                    }
                    else
                    {
                        var imagetest = new ApplianceImages();
                        imagetest.ApplianceImage = Request.Scheme.ToString() + "://" + Request.Host.ToString() + "/Default_Image/Default_Image.jpg";
                        Appliance.ApplianceImage.Add(imagetest);

                    }
                }

                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Appliance Detail", data = objList });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }// end

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateApplianceAsync([FromForm] GetAppliances appliance)
        {
            try
            {
                if (ModelState.IsValid )
                {
                    if(!_db.Vendor.Any(x=>x.SupplierId == appliance.VendorId))
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Vendor not found" });
                    }
                    var result = new Appliance 
                    {
                        CategoryId = appliance.CategoryId,
                        ApplianceCode = appliance.ApplianceCode,
                        BrandId = appliance.BrandId,
                        PruchaseDate = appliance.PruchaseDate,
                        WarrantPeriod = appliance.WarrantPeriod,
                        VendorId = appliance.VendorId
                    };
                    _db.Appliance.Add(result);
                    _db.SaveChanges();

                    var applianceImage = new List<string>();
                    foreach (var formFile in appliance.ApplianceImage)
                    {
                        var img_result = new Images
                        {
                            ApplianceID = result.ApplianceId
                        };

                        if (formFile.Length > 0)
                        {
                            var path = $"wwwroot\\Images\\";
                            var ImagePath = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "-" + formFile.FileName;
                            var imagePath = path + ImagePath;
                            // full path to file in temp location
                            var filePath = Path.GetFullPath(imagePath); //we are using Temp file name just for the example. Add your own file path.

                            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                            applianceImage.Add("/Images/" + ImagePath);

                            img_result.ApplianceImage = "/Images/" + ImagePath;
                            _db.Images.Add(img_result);
                        }
                        _db.SaveChanges();
                    }
                   
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record insert successfully" });
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
        [Route("Vendor")]
        public ActionResult GetVendor()
        {
            try
            {
                var objList = _db.Vendor.Where(x => x.IsDelete == 0).OrderByDescending(p=>p.SupplierId).Select(p => new { p.SupplierId, p.SupplierName, p.SupplierNumber });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Vendor Detail", data = objList });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Delete")]
        public ActionResult SoftDeleteAppliance(int id)
        {
            try
            {
                var entity = _db.Appliance.Where(x => x.ApplianceId == id).FirstOrDefault();
                if (entity.IsAssign == 1)
                {
                    return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliances is assigned, Cannot delete this item." });
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
        [Route("IsActive")]
        public ActionResult IsActiveAppliance(int id)
        {
            try
            {
                var entity = _db.Appliance.Where(x => x.ApplianceId == id).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    entity.IsActive = 1;
                    _db.SaveChanges();
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "IsActive Update successfully" });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Update")]

        public ActionResult UpdateAppliance([FromBody] Appliance appliance)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_db.Appliance.Where(c => c.IsDelete == 0 ).Any(x => x.ApplianceId == appliance.ApplianceId))
                    {
                        _db.Appliance.Update(appliance);
                        _db.SaveChanges();

                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Update successfully" });
                    }
                    return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Eater Valid ID" });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." }); ;
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message =e.InnerException.Message });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("ApplianceGetById")]
        public ActionResult ApplianceGetById(int id)
        {
            try
            {


                var objList = (from c in _db.Appliance
                               join b in _db.Vendor
                                on c.VendorId equals b.SupplierId
                                join br in _db.BrandDetail
                                on c.BrandId equals br.BrandId
                               join cat in _db.CategoryDetail
                                on c.CategoryId equals cat.CategoryId
                               where
                            c.IsDelete == 0 && c.ApplianceId == id
                               select new AllAppliance_val
                               {
                                   ApplianceId = c.ApplianceId,
                                   ApplianceCode = c.ApplianceCode,
                                   ApplianceName = cat.CategoryName,
                                   CategoryId = cat.CategoryId,
                                   BrandName = br.BrandName,
                                   BrandId = br.BrandId,
                                   PurchaseDate = c.PruchaseDate,
                                   WarrantPeriod = c.WarrantPeriod,
                                   SupplierId = b.SupplierId,
                                   SupplierName = b.SupplierName,
                                   SupplierNumber = b.SupplierNumber
                               }).OrderByDescending(p => p.ApplianceId).ToList();

                foreach (var Appliance in objList)
                {
                    var img_obj = (from d in _db.Images
                                   where d.ApplianceID == Appliance.ApplianceId
                                   select new ApplianceImages
                                   {
                                       ApplianceImage = d.ApplianceImage,
                                       ApplianceImageId = d.ApplianceImageId
                                   }).OrderByDescending(p => p.ApplianceImageId).ToList();

                    if (img_obj != null && img_obj.Count() > 0)
                    {
                        foreach (var image in img_obj)
                        {
                            var imagetest = new ApplianceImages();

                            imagetest.ApplianceImageId = image.ApplianceImageId;
                            imagetest.ApplianceImage = Request.Scheme.ToString() + "://" + Request.Host.ToString() + image.ApplianceImage;
                            Appliance.ApplianceImage.Add(imagetest);
                        }
                    }
                    else
                    {
                        var imagetest = new ApplianceImages();
                        imagetest.ApplianceImage = Request.Scheme.ToString() + "://" + Request.Host.ToString() + "/Default_Image/Default_Image.jpg";
                        Appliance.ApplianceImage.Add(imagetest);

                    }
                }


                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Issue Detail", data = objList });
            }

            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }

        ///Api/Appliance/InsertImgages
        //Insert Appliance id and images 
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("InsertImages")]
        public async Task<ActionResult> InsertImgagesAsync([FromForm] GetAppliances appliance)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new Appliance
                    {
                        ApplianceId = appliance.ApplianceId
                    };

                    var applianceImages = new List<string>();
                    foreach (var formFile in appliance.ApplianceImage)
                    {
                        var img_result = new Images
                        {
                            ApplianceID = result.ApplianceId,
                        };

                        if (formFile.Length > 0)
                        {
                            var path = $"wwwroot\\Images\\";
                            var ImagePath = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "-" + formFile.FileName;
                            var imagePath = path + ImagePath;
                            // full path to file in temp location
                            var filePath = Path.GetFullPath(imagePath); //we are using Temp file name just for the example. Add your own file path.

                            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                            applianceImages.Add("/Images/" + ImagePath);

                            img_result.ApplianceImage = "/Images/" + ImagePath;

                            _db.Images.Add(img_result);
                        }
                        _db.SaveChanges();
                    }

                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record insert successfully" });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }


        //All brand API

        //  Api/Appliance/InsertBrandName
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("InsertBrandName")]
        public ActionResult CreateBrandName(BrandDetail brand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_db.BrandDetail.Any(b => b.BrandName == brand.BrandName))
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Brand Name Already Exist" });
                    }
                    else
                    {
                        _db.BrandDetail.Add(brand);
                        _db.SaveChanges();
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record insert successfully" });
                    }
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        //  Api/Appliance/GetBrandName
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetBrandName")]
        public ActionResult GetBrandName()
        {
            try
            {
                var objList = _db.BrandDetail.Where(x => x.IsDelete == 0).OrderByDescending(p => p.BrandId).Select(p => new { p.BrandId, p.BrandName });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Brand Detail", data = objList });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        //  Api/Appliance/GetBrandDetailById
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetBrandDetailById")]
        public ActionResult GetBrandDetailById(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id != null && id != 0)
                    {
                        var objList = _db.BrandDetail.Where(x => x.BrandId == id && x.IsDelete == 0).OrderByDescending(p => p.BrandId).Select(p => new { p.BrandId, p.BrandName });

                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Brand Detail by id", data = objList });
                    }
                    else
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "id is null or 0" });
                    }
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });

            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

     
        // Soft Delete Brand Detail 
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("SoftDeleteBrandDetail")]
        public ActionResult SoftDeleteBrandDetail(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = _db.BrandDetail.Where(x => x.BrandId == id).FirstOrDefault();
                    if (entity != null)
                    {
                        _db.BrandDetail.Remove(entity);
                        _db.SaveChanges();
                    }
                    else
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Not Found" });
                    }
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Delete successfully" });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        // Soft Delete Brand Detail 
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("multiplesSoftDeleteBrandDetail")]
        public ActionResult multiplesSoftDeleteBrandDetail(IEnumerable<int> id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach(int ids in id)
                    {
                        var entity = _db.BrandDetail.Where(x => x.BrandId == ids).FirstOrDefault();
                        if(entity != null)
                        {
                            if (_db.Appliance.Any(x => x.BrandId == entity.BrandId))
                            {
                                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = entity.BrandName + " is assigned in Appliance you can't delete" });
                            }
                            _db.BrandDetail.Remove(entity);
                        }
                        else
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = ids + " brand id not found in DB" });
                        }
                    }
                    _db.SaveChanges();
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Delete successfully" });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }


        //  Api/Appliance/InsertBrandName
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("UpdateBrandDetail")]
        public ActionResult UpdateBrandDetail(BrandDetail brand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_db.BrandDetail.Any(b => b.BrandName == brand.BrandName))
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Brand Name Already Exist" });
                    }
                    else
                    {
                        _db.BrandDetail.Update(brand);
                        _db.SaveChanges();
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Update successfully" });
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
        [HttpPost]
        [Route("MultipleDelete")]
        public ActionResult DeleteSelected(IEnumerable<int> id)
        {
            try
            {
                foreach (int ids in id)
                {
                    var entity = _db.Appliance.Where(x => x.ApplianceId == ids).FirstOrDefault();
                    if (entity != null)
                    {
                        if (entity.IsAssign == 1)
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.OK, message = entity.ApplianceCode + "You cant remove appliances as it is assigned to user." });
                        }
                        if (entity.IsDelete == 0)
                        {
                            entity.IsDelete = 1;
                        }
                        else
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Already deleted" });
                        }
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
        [HttpGet]
        [Route("IsAssigen")]
        public ActionResult GetIsAssigenappliance()
        {
            try
            {
                var objList = _db.Appliance.Where(x => x.IsAssign == 1 && x.IsDelete==0).Select(p => new { p.ApplianceId });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Assign Appliance Detail", data = objList });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        //Api/Appliance/DeleteImages
        [Authorize(Roles = "Admin")]
        //[HttpDelete]
        [HttpPost]
        [Route("DeleteImages")]
        public IActionResult DeleteImagesBox(IEnumerable<int> id)
        {
            try
            {
                foreach (int ids in id)
                {
                    var imgs = _db.Images.Where(x => x.ApplianceImageId == ids).FirstOrDefault();
                    if (imgs == null)
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Images not found in database." });
                    }
                    var img = _db.Images.Where(x => x.ApplianceImageId == ids).FirstOrDefault();

                    var path = $"wwwroot//";
                    var imagePath = path + img.ApplianceImage;
                    var formFile = img.ApplianceImage;
                    var filePath = Path.GetFullPath(imagePath);
                    //check file Exists or not
                    //if (System.IO.File.Exists(filePath))
                    //{
                       // System.IO.File.Delete(filePath);
                        _db.Images.Remove(imgs);
                        _db.SaveChanges();
                       
                    //}
                    //else
                    //{
                    //    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "File not found in folder" });
                    //}
                }
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Delete successfully" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

    }


}
