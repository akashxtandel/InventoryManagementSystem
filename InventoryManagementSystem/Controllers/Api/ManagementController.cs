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
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using static Grpc.Core.Metadata;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace InventoryManagementSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly InventoryManagementContext _db;

        public ManagementController(InventoryManagementContext db)
        {
            _db = db;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var objList = _db.Management.Where(x => x.IsDelete == 0 && x.Role == "Employee").OrderByDescending(p => p.ManagementId).Select(p => new { p.ManagementId, p.EmployeeName, p.EmployEmailId, p.Password, p.Designation, p.AssignProjectName, p.DateOfIssue, p.Role });
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "User Detail", data = objList });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateManagement(ManagementDTO? management)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_db.Management.Any(x => x.EmployEmailId == management.EmployEmailId))
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Email Already exist" });
                    }
                    if (management.Role == null)
                    {
                        management.Role = "Employee";
                    }
                    var result = new Management
                    {
                        EmployeeName = management.EmployeeName,
                        EmployEmailId = management.EmployEmailId,
                        Password = Encrypt_Password(management.Password),
                        Designation = management.Designation,
                        AssignProjectName = management.AssignProjectName,
                        DateOfIssue = management.DateOfIssue,
                        Role = management.Role,
                        Monitor = management.MonitorId,
                        Cpu = management.CpuId,
                        Keybord = management.KeybordId,
                        Mouse = management.MonitorId,
                        Headphone = management.HeadphoneId,
                        Camera = management.CameraId,
                        // Others = management.OthersId
                    };  
                    // add management data
                    _db.Management.Add(result);
                    //check Appliance 
                    if (management.MonitorId != null)
                    {
                        var monitor = _db.Appliance.FirstOrDefault(x => x.ApplianceId == result.Monitor);

                        if (monitor == null) {  return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = result.Monitor + " Appliance not found" });}

                        if (monitor.IsAssign == 1)
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliance already Assigned" });
                        }
                        monitor.IsAssign = 1;
                        _db.Appliance.Update(monitor);
                    }

                    if (management.CpuId != null)
                    {
                        var cpu = _db.Appliance.FirstOrDefault(x => x.ApplianceId == result.Cpu);

                        if(cpu == null){ return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = result.Cpu + " Appliance not found" }); }

                        if (cpu.IsAssign == 1)
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliance already Assigned" });
                        }
                        cpu.IsAssign = 1;
                        _db.Appliance.Update(cpu);
                    }

                    if (management.KeybordId != null)
                    {
                        var Keybord = _db.Appliance.FirstOrDefault(x => x.ApplianceId == result.Keybord);

                        if (Keybord == null) { return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = result.Keybord + " Appliance not found" }); }

                        if (Keybord.IsAssign == 1)
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliance already Assigned" });
                        }
                        Keybord.IsAssign = 1;
                        _db.Appliance.Update(Keybord);
                    }

                    if (management.MouseId != null)
                    {
                        var Mouse = _db.Appliance.FirstOrDefault(x => x.ApplianceId == result.Mouse);

                        if (Mouse == null) { return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = result.Mouse + " Appliance not found" }); }

                        if (Mouse.IsAssign == 1)
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliance already Assigned" });
                        }
                        Mouse.IsAssign = 1;
                        _db.Appliance.Update(Mouse);
                    }

                    if (management.HeadphoneId != null)
                    {
                        var Headphone = _db.Appliance.FirstOrDefault(x => x.ApplianceId == result.Headphone);

                        if (Headphone == null) { return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = result.Headphone + " Appliance not found" }); }

                        if (Headphone.IsAssign == 1)
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliance already Assigned" });
                        }
                        Headphone.IsAssign = 1;
                        _db.Appliance.Update(Headphone);
                    }

                    if (management.CameraId != null)
                    {
                        var Camera = _db.Appliance.FirstOrDefault(x => x.ApplianceId == result.Camera);

                        if (Camera == null) { return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = result.Camera + " Appliance not found" }); }

                        if (Camera.IsAssign == 1)
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliance already Assigned" });
                        }
                        Camera.IsAssign = 1;
                        _db.Appliance.Update(Camera);
                    }

                    //Created others appliance
                    if (management.OthersId != null)
                    {
                        foreach (var item in management.OthersId)
                        {
                           var  getApplinace  = _db.Appliance.Where(x=>x.ApplianceId==item).FirstOrDefault();
                            if(getApplinace == null)
                            {
                                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliance not found" });
                            }
                            if (getApplinace.IsAssign == 1)
                            {
                                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliance already Assigned" });
                            }
                            getApplinace.IsAssign= 1;
                            _db.Appliance.Update(getApplinace);

                            var others = new OthersAppliance();
                            others.Appliance_id = item;
                            others.Management_id = management.ManagementId;
                            _db.Other_Applince.Add(others);
                        }   
                    }
                    _db.SaveChanges();
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
        [HttpPost]
        [Route("Update")]
        public ActionResult UpdateManagement([FromBody] Management management)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] err_msg = { "ok", "ok", "ok", "ok", "ok", "ok","ok" };

                    if (_db.Management.Where(c => c.IsDelete == 0).Any(x => x.ManagementId == management.ManagementId))
                    {
                        var existingManagement = _db.Management.FirstOrDefault(x => x.ManagementId == management.ManagementId);

                        existingManagement.EmployeeName = management.EmployeeName;
                        existingManagement.EmployEmailId = management.EmployEmailId;
                        existingManagement.Password = Encrypt_Password(management.Password);
                        existingManagement.Designation = management.Designation;
                        existingManagement.AssignProjectName = management.AssignProjectName;
                        existingManagement.DateOfIssue = management.DateOfIssue;
                        if (management.Role == null)
                        {
                            management.Role = "Employee";
                        }
                        //monitor 0
                        if (existingManagement.Monitor != management.Monitor && existingManagement.Monitor != null)
                        {
                            var existingMonitor = _db.Appliance.FirstOrDefault(x => x.ApplianceId == existingManagement.Monitor);
                            if (existingManagement != null)
                            {
                                var Monitor = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Monitor);
                                if (Monitor != null)
                                {
                                    if (Monitor.IsDelete == 1 || Monitor.IsAssign == 1) { err_msg[0] = management.Monitor + " : Appliance isDelete or IsAssign"; }
                                    Monitor.IsAssign = 1;
                                    existingMonitor.IsAssign = 0;
                                    existingManagement.Monitor = management.Monitor;
                                    _db.Appliance.Update(Monitor);
                                    _db.Appliance.Update(existingMonitor);
                                }
                                else
                                {
                                    err_msg[0] = "ApplianceId : " + management.Monitor + " Not found ";
                                }
                            }
                            else
                            {
                                err_msg[0] = "existingManagement id " + management.Monitor + " : Not found in Db ";
                            }
                        }
                        else if (existingManagement.Monitor == null)
                        {
                            var Monitor = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Monitor && x.IsAssign == 0 && x.IsDelete == 0);
                            if (Monitor != null)
                            {
                                if (Monitor.IsDelete == 1 || Monitor.IsAssign == 1) { err_msg[0] = management.Monitor + " : Appliance isDelete or IsAssign"; }
                                Monitor.IsAssign = 1;
                                existingManagement.Monitor = management.Monitor;
                            }
                            else
                            {
                                err_msg[0] = "ApplianceId : " + management.Monitor + " Not found ";
                            }
                        }
                        //Mouse 1
                        if (existingManagement.Mouse != management.Mouse && existingManagement.Mouse != null)
                        {
                            var existingMouse = _db.Appliance.FirstOrDefault(x => x.ApplianceId == existingManagement.Mouse);
                            if (existingManagement != null)
                            {
                                var Mouse = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Mouse);
                                if (Mouse != null)
                                {
                                    if (Mouse.IsDelete == 1 || Mouse.IsAssign == 1) { err_msg[1] = management.Mouse + " : Appliance isDelete or IsAssign"; }
                                    Mouse.IsAssign = 1;
                                    existingMouse.IsAssign = 0;
                                    existingManagement.Mouse = management.Mouse;
                                    _db.Appliance.Update(Mouse);
                                    _db.Appliance.Update(existingMouse);
                                }
                                else
                                {
                                    err_msg[1] = "ApplianceId : " + management.Mouse + " Not found ";
                                }
                            }
                            else
                            {
                                err_msg[1] = "existingManagement id " + management.Mouse + " : Not found in Db ";
                            }
                        }
                        else if (existingManagement.Mouse == null)
                        {
                            var Mouse = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Mouse && x.IsAssign == 0 && x.IsDelete == 0);
                            if (Mouse != null)
                            {
                                if (Mouse.IsDelete == 1 || Mouse.IsAssign == 1) { err_msg[1] = management.Mouse + " : Appliance isDelete or IsAssign"; }
                                Mouse.IsAssign = 1;
                                existingManagement.Mouse = management.Mouse;
                            }
                            else
                            {
                                err_msg[1] = "ApplianceId : " + management.Mouse + " Not found ";
                            }
                        }

                        //CPU 2
                        if (existingManagement.Cpu != management.Cpu && existingManagement.Cpu != null)
                        {
                            var existingCpu = _db.Appliance.FirstOrDefault(x => x.ApplianceId == existingManagement.Cpu);
                            if (existingManagement != null)
                            {
                                var Cpu = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Cpu && x.IsAssign == 0 && x.IsDelete == 0);
                                if (Cpu != null)
                                {
                                    if (Cpu.IsDelete == 1 || Cpu.IsAssign == 1) { err_msg[2] = management.Cpu + " : Appliance isDelete or IsAssign"; }
                                    Cpu.IsAssign = 1;
                                    existingCpu.IsAssign = 0;
                                    existingManagement.Cpu = management.Cpu;
                                    _db.Appliance.Update(Cpu);
                                    _db.Appliance.Update(existingCpu);
                                }
                                else
                                {
                                    err_msg[2] = "ApplianceId : " + management.Cpu + " Not found ";
                                }
                            }
                            else
                            {
                                err_msg[2] = "existingManagement id " + management.Cpu + " : Not found in Db ";
                            }
                        }
                        else if (existingManagement.Cpu == null)
                        {
                            var Cpu = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Cpu && x.IsAssign == 0 && x.IsDelete == 0);
                            if (Cpu != null)
                            {
                                if (Cpu.IsDelete == 1 || Cpu.IsAssign == 1) { err_msg[2] = management.Cpu + " : Appliance isDelete or IsAssign"; }
                                Cpu.IsAssign = 1;
                                existingManagement.Cpu = management.Cpu;
                            }
                            else
                            {
                                err_msg[2] = "ApplianceId : " + management.Cpu + " Not found ";
                            }
                        }

                        //Keyboard 3

                        if (existingManagement.Keybord != management.Keybord && existingManagement.Keybord != null)
                        {
                            var existingKeybord = _db.Appliance.FirstOrDefault(x => x.ApplianceId == existingManagement.Keybord);
                            if (existingManagement != null)
                            {
                                var Keybord = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Keybord && x.IsAssign == 0 && x.IsDelete == 0);
                                if (Keybord != null)
                                {
                                    if (Keybord.IsDelete == 1 || Keybord.IsAssign == 1) { err_msg[3] = management.Keybord + " : Appliance isDelete or IsAssign"; }
                                    Keybord.IsAssign = 1;
                                    existingKeybord.IsAssign = 0;
                                    existingManagement.Keybord = management.Keybord;
                                    _db.Appliance.Update(Keybord);
                                    _db.Appliance.Update(existingKeybord);
                                }
                                else
                                {
                                    err_msg[3] = "ApplianceId : " + management.Keybord + " Not found ";
                                }
                            }
                            else
                            {
                                err_msg[3] = "existingManagement id " + management.Keybord + " : Not found in Db ";
                            }
                        }
                        else if (existingManagement.Keybord == null)
                        {
                            var Keybord = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Keybord && x.IsAssign == 0 && x.IsDelete == 0);
                            if (Keybord != null)
                            {
                                if (Keybord.IsDelete == 1 || Keybord.IsAssign == 1) { err_msg[3] = management.Keybord + " : Appliance isDelete or IsAssign"; }
                                Keybord.IsAssign = 1;
                                existingManagement.Keybord = management.Keybord;
                            }
                            else
                            {
                                err_msg[3] = management.Keybord + " : Not found Or Appliance IsAssign already or isDelete";
                            }
                        }


                        // HeadPhone 4

                        if (existingManagement.Headphone != management.Headphone && existingManagement.Headphone != null)
                        {
                            var existingHeadphone = _db.Appliance.FirstOrDefault(x => x.ApplianceId == existingManagement.Headphone);
                            if (existingManagement != null)
                            {
                                var Headphone = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Headphone && x.IsAssign == 0 && x.IsDelete == 0);
                                if (Headphone != null)
                                {
                                    if (Headphone.IsDelete == 1 || Headphone.IsAssign == 1) { err_msg[4] = management.Headphone + " : Appliance isDelete or IsAssign"; }
                                    Headphone.IsAssign = 1;
                                    existingHeadphone.IsAssign = 0;
                                    existingManagement.Headphone = management.Headphone;
                                    _db.Appliance.Update(Headphone);
                                    _db.Appliance.Update(existingHeadphone);
                                }
                                else
                                {
                                    err_msg[4] = "ApplianceId : " + management.Headphone + " Not found ";
                                }
                            }
                            else
                            {
                                err_msg[4] = "existingManagement id " + management.Headphone + " : Not found in Db ";
                            }
                        }
                        else if (existingManagement.Headphone == null)
                        {
                            var Headphone = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Headphone && x.IsAssign == 0 && x.IsDelete == 0);
                            if (Headphone != null)
                            {
                                if (Headphone.IsDelete == 1 || Headphone.IsAssign == 1) { err_msg[4] = management.Headphone + " : Appliance isDelete or IsAssign"; }
                                Headphone.IsAssign = 1;
                                existingManagement.Headphone = management.Headphone;
                            }
                            else
                            {
                                err_msg[4] = "ApplianceId : " + management.Headphone + " Not found ";
                            }
                        }


                        // Camera 5

                        if (existingManagement.Camera != management.Camera && existingManagement.Camera != null)
                        {
                            var existingCamera = _db.Appliance.FirstOrDefault(x => x.ApplianceId == existingManagement.Camera);
                            if (existingManagement != null)
                            {
                                var Camera = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Camera && x.IsAssign == 0 && x.IsDelete == 0);
                                if (Camera != null)
                                {
                                    if (Camera.IsDelete == 1 || Camera.IsAssign == 1) { err_msg[5] = management.Camera + " : Appliance isDelete or IsAssign"; }
                                    Camera.IsAssign = 1;
                                    existingCamera.IsAssign = 0;
                                    existingManagement.Camera = management.Camera;
                                    _db.Appliance.Update(Camera);
                                    _db.Appliance.Update(existingCamera);
                                }
                                else
                                {
                                    err_msg[5] = "ApplianceId : " + management.Camera + " Not found ";
                                }
                            }
                            else
                            {
                                err_msg[5] = management.Camera + " : Not found " + management.Camera + " in existingManagement from db";
                            }
                        }
                        else if (existingManagement.Camera == null)
                        {
                            var Camera = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Camera && x.IsAssign == 0 && x.IsDelete == 0);
                            if (Camera != null)
                            {
                                if (Camera.IsDelete == 1 || Camera.IsAssign == 1) { err_msg[5] = management.Camera + " : Appliance isDelete or IsAssign"; }
                                Camera.IsAssign = 1;
                                existingManagement.Camera = management.Camera;
                            }
                            else
                            {
                                err_msg[5] = "ApplianceId : " + management.Camera + " Not found ";
                            }
                        }
                        // others 6
                         
                        //if (existingManagement.Others != management.Others && existingManagement.Others != null)
                        //{
                        //    var existingOthers = _db.Appliance.FirstOrDefault(x => x.ApplianceId == existingManagement.Others);
                        //    if (existingManagement != null)
                        //    {
                        //        var Others = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Others && x.IsAssign == 0 && x.IsDelete == 0);
                        //        if (Others != null)
                        //        {
                        //            if (Others.IsDelete == 1 || Others.IsAssign == 1) { err_msg[6] = management.Others + " : Appliance isDelete or IsAssign"; }
                        //            Others.IsAssign = 1;
                        //            existingOthers.IsAssign = 0;
                        //            existingManagement.Others = management.Others;
                        //            _db.Appliance.Update(Others);
                        //            _db.Appliance.Update(existingOthers);
                        //        }
                        //        else
                        //        {
                        //            err_msg[6] = "ApplianceId : " + management.Others + " Not found ";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        err_msg[6] = management.Others + " : Not found " + management.Others + " in existingManagement from db";
                        //    }
                        //}
                        //else if (existingManagement.Others == null)
                        //{
                        //    var Others = _db.Appliance.FirstOrDefault(x => x.ApplianceId == management.Others && x.IsAssign == 0 && x.IsDelete == 0);
                        //    if (Others != null)
                        //    {
                        //        if (Others.IsDelete == 1 || Others.IsAssign == 1) { err_msg[6] = management.Others + " : Appliance isDelete or IsAssign"; }
                        //        Others.IsAssign = 1;
                        //        existingManagement.Others = management.Others;
                        //    }
                        //    else
                        //    {
                        //        err_msg[6] = "ApplianceId : " + management.Others + " Not found ";
                        //    }
                        //}
                        _db.Management.Update(existingManagement);
                        _db.SaveChanges();

                        return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Record Update successfully", err_msg_for_postman = err_msg });
                    }
                    return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Enter Valid ID" });
                }
                return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Something wrong." }); ;
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        [Route("EmployeeGetById")]
        public ActionResult EmployeeGetById(int id)
        {
            try
            {
                ManagementDTO getEmployeeGetById = new ManagementDTO();

                var employee = (from c in _db.Management
                                where c.ManagementId == id
                                select new
                                {
                                    c.ManagementId,
                                    c.EmployEmailId,
                                    c.EmployeeName,
                                    c.Designation,
                                    c.DateOfIssue,
                                    c.AssignProjectName,
                                    c.Password,
                                    c.Role,
                                    c.Monitor,
                                    c.Cpu,
                                    c.Keybord,
                                    c.Mouse,
                                    c.Headphone,
                                    c.Camera,
                                    c.Others,
                                    c.IsDelete
                                }).FirstOrDefault();
                /*var monitorlist = (from c in _db.Management
                                   join b in _db.Appliance on c.Monitor equals b.ApplianceId 
                                   where c.ManagementId == id
                                   select new
                                   {
                                       c.ManagementId
                                   }).FirstOrDefault();
                var keyBordlist = (from c in _db.Management
                                   join b in _db.Appliance on c.Keybord equals b.ApplianceId
                                   where c.ManagementId == id
                                   select new
                                   {
                                       c.ManagementId
                                   }).FirstOrDefault();
                var mouselist = (from c in _db.Management
                                 join b in _db.Appliance on c.Mouse equals b.ApplianceId
                                 where c.ManagementId == id
                                 select new
                                 {
                                     c.ManagementId
                                 }).FirstOrDefault();
                var cpulist = (from c in _db.Management
                               join b in _db.Appliance on c.Cpu equals b.ApplianceId
                               where c.ManagementId == id
                               select new
                               {
                                   b.ApplianceId
                               }).FirstOrDefault();
                var headphonelist = (from c in _db.Management
                                     join b in _db.Appliance on c.Headphone equals b.ApplianceId
                                     where c.ManagementId == id
                                     select new
                                     {
                                         c.ManagementId
                                     }).FirstOrDefault();
                var camaralist = (from c in _db.Management
                                  join b in _db.Appliance on c.Camera equals b.ApplianceId
                                  where c.ManagementId == id
                                  select new
                                  {
                                      c.ManagementId
                                  }).FirstOrDefault();*/
                if (employee.IsDelete == 1)
                {
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "User soft deleted" });
                }
                getEmployeeGetById.ManagementId = employee.ManagementId;
                getEmployeeGetById.EmployEmailId = employee.EmployEmailId;
                getEmployeeGetById.EmployeeName = employee.EmployeeName;
                getEmployeeGetById.Password = Decrypt_Password(employee.Password);
                getEmployeeGetById.Designation = employee.Designation;
                getEmployeeGetById.DateOfIssue = employee.DateOfIssue;
                getEmployeeGetById.AssignProjectName = employee.AssignProjectName;

                getEmployeeGetById.MonitorId = employee.Monitor;
                getEmployeeGetById.CpuId = employee.Cpu;
                getEmployeeGetById.MouseId = employee.Mouse;
                getEmployeeGetById.KeybordId = employee.Keybord;
                getEmployeeGetById.HeadphoneId = employee.Headphone;
                getEmployeeGetById.CameraId = employee.Camera;
                //getEmployeeGetById.OthersId = employee.Others.ToString();

                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "EmployeeDetail", data = getEmployeeGetById });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });

            }

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public string Decrypt_Password(string password)
        {
            string key = "1prt56";
            byte[] SrctArray;
            byte[] DrctArray = Convert.FromBase64String(password);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objmdcript = new MD5CryptoServiceProvider();
            SrctArray = objmdcript.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objmdcript.Clear();
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateDecryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(DrctArray, 0, DrctArray.Length);
            objt.Clear();
            return UTF8Encoding.UTF8.GetString(resArray);
        }

        [HttpGet]
        [Route("Moniter")]
        public IActionResult GetMoniter()
        {
            try
            {
                var objlist = from c in _db.Appliance
                              where
                              //EF.Functions.Like(c.ApplianceCode, "%1%")//1 Monitor
                              c.ApplianceCode=="1"
                              select c;
                var moniter = objlist.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.ApplianceId);
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Moniter Detail", data = moniter });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }
        [HttpGet]
        [Route("CPU")]
        public IActionResult GetCPU()
        {
            try
            {
                var objlist = from c in _db.Appliance
                              where
                              //EF.Functions.Like(c.ApplianceCode, "%6%") //6 CPU
                              c.ApplianceCode == "6"
                              select c;
                var cpu = objlist.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.ApplianceId);
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "CPU Detail", data = cpu });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }
        [HttpGet]
        [Route("KeyBord")]
        public IActionResult GetKeyBoard()
        {
            try
            {
                var objlist = from c in _db.Appliance
                              where
                              // EF.Functions.Like(c.ApplianceCode, "%2%")// 2 Keyboard
                              c.ApplianceCode=="2"
                              select c;
                var Keyboard = objlist.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.ApplianceId);
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "KeyBoard Detail", data = Keyboard });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }
        [HttpGet]
        [Route("Mouse")]
        public IActionResult GetMouse()
        {
            try
            {
                var objlist = from c in _db.Appliance
                              where
                              // EF.Functions.Like(c.ApplianceCode, "%3%")//3 Mouse
                              c.ApplianceCode =="3"
                              select c;
                var Mouse = objlist.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.ApplianceId);
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Mouse Detail", data = Mouse });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }
        [HttpGet]
        [Route("Headphone")]
        public IActionResult GetHeadphone()
        {
            try
            {
                var objlist = from c in _db.Appliance
                              where
                              //EF.Functions.Like(c.ApplianceCode, "%5%")//5 Headphone
                              c.ApplianceCode=="5"
                              select c;
                var Headphone = objlist.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.ApplianceId);
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Headphone Detail", data = Headphone });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }
        [HttpGet]
        [Route("Camera")]
        public IActionResult GetCamera()
        {
            try
            {
                var objlist = from c in _db.Appliance
                              where
                              //EF.Functions.Like(c.ApplianceCode, "%4%")//4 Camera
                              c.ApplianceCode=="4"
                              select c;
                var Camera = objlist.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.ApplianceId);
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Camera Detail", data = Camera });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }

        [HttpGet]
        [Route("Others")]
        public IActionResult GetOthers()
        {
            try
            {
                var objlist = from c in _db.Appliance
                              where
                              //EF.Functions.Like(c.ApplianceCode, "%%")// 8 others
                              c.ApplianceCode =="8"
                              select c;
                var Others = objlist.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0 ).OrderByDescending(p => p.ApplianceId );
                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Others Detail", data = Others });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }

        [HttpGet]
        [Route("GetAllAppliances")]
        public IActionResult GetAllAppliances()
        {
            try
            {
                var Moniterlist = from c in _db.Appliance
                                  join br in _db.BrandDetail
                                  on c.BrandId equals br.BrandId
                                  where
                                  //EF.Functions.Like(c.CategoryId.ToString(), "%1%") //1 Monitor
                                  c.CategoryId==1 
                                  select new
                                  {
                                      c.ApplianceId,
                                      c.CategoryId,
                                      c.ApplianceCode,
                                      c.ApplianceImage,
                                      c.BrandId,
                                      c.PruchaseDate,
                                      c.WarrantPeriod,
                                      c.VendorId,
                                      br.BrandName,
                                      c.IsDelete,
                                      c.IsActive,
                                      c.IsAssign
                                  };

                var keyboardList = from c in _db.Appliance
                                   join br in _db.BrandDetail
                                   on c.BrandId equals br.BrandId
                                   where
                                   //EF.Functions.Like(c.CategoryId.ToString(), "%2%")  // 2 Keyboard
                                    c.CategoryId == 2
                                   select new
                                   {
                                       c.ApplianceId,
                                       c.CategoryId,
                                       c.ApplianceCode,
                                       c.ApplianceImage,
                                       c.BrandId,
                                       c.PruchaseDate,
                                       c.WarrantPeriod,
                                       c.VendorId,
                                       br.BrandName,
                                       c.IsDelete,
                                       c.IsActive,
                                       c.IsAssign
                                   };

                var cpuList = from c in _db.Appliance
                              join br in _db.BrandDetail
                              on c.BrandId equals br.BrandId
                              where
                              //EF.Functions.Like(c.CategoryId.ToString(), "%6%") //6 CPU
                              c.CategoryId == 6
                              select new
                              {
                                  c.ApplianceId,
                                  c.CategoryId,
                                  c.ApplianceCode,
                                  c.ApplianceImage,
                                  c.BrandId,
                                  c.PruchaseDate,
                                  c.WarrantPeriod,
                                  c.VendorId,
                                  br.BrandName,
                                  c.IsDelete,
                                  c.IsActive,
                                  c.IsAssign
                              };
                var MouseList = from c in _db.Appliance
                                join br in _db.BrandDetail
                                on c.BrandId equals br.BrandId
                                where
                                //  EF.Functions.Like(c.CategoryId.ToString(), "%3%")//3 Mouse
                                 c.CategoryId == 3
                                select new
                                {
                                    c.ApplianceId,
                                    c.CategoryId,
                                    c.ApplianceCode,
                                    c.ApplianceImage,
                                    c.BrandId,
                                    c.PruchaseDate,
                                    c.WarrantPeriod,
                                    c.VendorId,
                                    br.BrandName,
                                    c.IsDelete,
                                    c.IsActive,
                                    c.IsAssign
                                };

                var HeadphoneList = from c in _db.Appliance
                                    join br in _db.BrandDetail
                                    on c.BrandId equals br.BrandId
                                    where
                                    // EF.Functions.Like(c.CategoryId.ToString(), "%5%")//5 Headphone
                                    c.CategoryId == 5
                                    select new
                                    {
                                        c.ApplianceId,
                                        c.CategoryId,
                                        c.ApplianceCode,
                                        c.ApplianceImage,
                                        c.BrandId,
                                        c.PruchaseDate,
                                        c.WarrantPeriod,
                                        c.VendorId,
                                        br.BrandName,
                                        c.IsDelete,
                                        c.IsActive,
                                        c.IsAssign
                                    };

                var CameraList = from c in _db.Appliance
                                 join br in _db.BrandDetail
                                 on c.BrandId equals br.BrandId
                                 where
                                 c.CategoryId == 4 
                                     ///EF.Functions.Like(c.CategoryId.ToString(), "%4%")//4 Camera
                                 select new
                                 {
                                     c.ApplianceId,
                                     c.CategoryId,
                                     c.ApplianceCode,
                                     c.ApplianceImage,
                                     c.BrandId,
                                     c.PruchaseDate,
                                     c.WarrantPeriod,
                                     c.VendorId,
                                     br.BrandName,
                                     c.IsDelete,
                                     c.IsActive,
                                     c.IsAssign
                                 };
                var OthersList = from c in _db.Appliance
                                 join br in _db.BrandDetail
                                 on c.BrandId equals br.BrandId
                                 join cat in _db.CategoryDetail
                                 on c.CategoryId equals cat.CategoryId
                                 where
                                     //EF.Functions.Like(c.CategoryId.ToString(), "%8%")//8 others
                                     cat.IsOthers == true
                                 select new
                                 {
                                     c.ApplianceId,
                                     c.CategoryId,
                                     c.ApplianceCode,
                                     c.ApplianceImage,
                                     c.BrandId,
                                     c.PruchaseDate,
                                     c.WarrantPeriod,
                                     c.VendorId,
                                     br.BrandName,
                                     c.IsDelete,
                                     c.IsActive,
                                     c.IsAssign
                                 };

                var Moniter = Moniterlist.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.IsAssign == 0);
                var cpu = cpuList.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.IsAssign == 0);
                var Keyboard = keyboardList.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.IsAssign == 0);
                var Camera = CameraList.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.IsAssign == 0);
                var Mouse = MouseList.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.IsAssign == 0);
                var Headphone = HeadphoneList.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.IsAssign == 0); 
                var Others = OthersList.AsEnumerable().Where(c => c.IsDelete == 0 && c.IsActive == 0).OrderByDescending(p => p.IsAssign == 0);

                return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Appliances Detail", data = new { Moniter, cpu, Camera, Keyboard, Mouse, Headphone, Others } });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = (int)HttpStatusCode.InternalServerError, message = e.InnerException.Message });
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Delete")]
        public ActionResult SoftDeleteManagement(int id)
        {
            try
            {
                var entity = _db.Management.Where(x => x.ManagementId == id).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    if (entity != null)
                    {
                        if (entity.IsDelete == 1)
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Already deleted" });
                        }
                        //else 
                        //if(
                        //    entity.Monitor != null ||
                        //    entity.Mouse != null ||
                        //    entity.Camera != null ||
                        //    entity.Cpu != null ||
                        //    entity.Keybord != null ||
                        //    entity.Headphone != null)
                        //{
                        //    return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Appliances isAssign this user" });
                        //}
                        entity.IsDelete = 1;
                    }
                    else
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = id + " [Management id not found]" });
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public string Encrypt_Password(string password)
        {
            string key = "1prt56";
            byte[] SrctArray;
            byte[] EnctArray = UTF8Encoding.UTF8.GetBytes(password);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();
            SrctArray = objcrpt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objcrpt.Clear();
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateEncryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);
            objt.Clear();
            return Convert.ToBase64String(resArray, 0, resArray.Length);
        }
        [HttpGet]
        [Route("IssueStatus")]
        public IActionResult IssueStatus(string emailId)
        {
            try
            {
                if (_db.Management.Where(c => c.IsDelete == 0).Any(x => x.EmployEmailId == emailId))
                {
                    var objlist = (from c in _db.Management
                                   join b in _db.Issue
                                   on c.ManagementId equals b.ManagementId
                                   where
                                   c.EmployEmailId.Contains(emailId) &&
                                   EF.Functions.Like(c.IsDelete.ToString(), "0")
                                   select new
                                   {
                                       b.IssueId,
                                       b.Appliances,
                                       b.IssueDescription,
                                       b.AdminResponse
                                   }).OrderByDescending(p => p.IssueId).ToList();
                    List<GetIssue> getIssues = new List<GetIssue>();
                    foreach (var item in objlist)
                    {
                        var getIssue = new GetIssue();
                        var numbers = item.Appliances.Split(',').Select(Int32.Parse).ToList();
                        foreach (var id in numbers)
                        {
                            var resolved = _db.Appliance.Where(x => x.ApplianceId == id).FirstOrDefault();
                            getIssue.ApplianceCode.Add(resolved.ApplianceCode);
                        }
                        getIssue.IssueDescription = item.IssueDescription;
                        getIssue.AdminResponse = item.AdminResponse;

                        getIssues.Add(getIssue);
                    }
                    return new JsonResult(new { status = (int)HttpStatusCode.OK, message = "Vendor Detail", data = getIssues });
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
        [Route("MultipleDelete")]

        public ActionResult DeleteSelected(IEnumerable<int> id)
        {
            try
            {
                foreach (int ids in id)
                {
                    var entity = _db.Management.Where(x => x.ManagementId == ids).FirstOrDefault();
                    if (entity != null && entity.IsDelete == 0)
                    {
                        var issue_found = _db.Issue.Where(x =>
                                                          x.IsDelete == 0 &&
                                                          x.ManagementId == entity.ManagementId &&
                                                         (x.Action == "Pending" || x.Action == "Accept")).FirstOrDefault();

                        var A_Monitor   = _db.Appliance.Where(x => x.IsDelete == 0 && x.ApplianceId == entity.Monitor   && x.IsAssign == 1).FirstOrDefault();
                        var A_Cpu       = _db.Appliance.Where(x => x.IsDelete == 0 && x.ApplianceId == entity.Cpu       && x.IsAssign == 1).FirstOrDefault();
                        var A_Mouse     = _db.Appliance.Where(x => x.IsDelete == 0 && x.ApplianceId == entity.Mouse     && x.IsAssign == 1).FirstOrDefault();
                        var A_Keyboard  = _db.Appliance.Where(x => x.IsDelete == 0 && x.ApplianceId == entity.Keybord   && x.IsAssign == 1).FirstOrDefault();
                        var A_Headphone = _db.Appliance.Where(x => x.IsDelete == 0 && x.ApplianceId == entity.Headphone && x.IsAssign == 1).FirstOrDefault();
                        var A_Camera    = _db.Appliance.Where(x => x.IsDelete == 0 && x.ApplianceId == entity.Camera    && x.IsAssign == 1).FirstOrDefault();
                        //var A_Others    = _db.Appliance.Where(x => x.IsDelete == 0 && x.ApplianceId == entity.Others && x.IsAssign == 1).FirstOrDefault();

                        if (issue_found == null)
                        {
                            if (entity.Monitor   != null && A_Monitor   != null) { entity.Monitor   = null;     A_Monitor.IsAssign = 0; }
                            if (entity.Cpu       != null && A_Cpu       != null) { entity.Cpu       = null;         A_Cpu.IsAssign = 0; }
                            if (entity.Mouse     != null && A_Mouse     != null) { entity.Mouse     = null;       A_Mouse.IsAssign = 0; }
                            if (entity.Keybord   != null && A_Keyboard  != null) { entity.Keybord   = null;    A_Keyboard.IsAssign = 0; }
                            if (entity.Headphone != null && A_Headphone != null) { entity.Headphone = null;   A_Headphone.IsAssign = 0; }
                            if (entity.Camera    != null && A_Camera    != null) { entity.Camera    = null;      A_Camera.IsAssign = 0; }
                            //if (entity.Others    != null && A_Others    != null) { entity.Others    = null;      A_Others.IsAssign = 0; }
                            entity.IsDelete = 1;
                        }
                        else
                        {
                            return new JsonResult(new { status = (int)HttpStatusCode.OK, message = entity.EmployeeName+ " You cant remove the user as they have a pending issue." });
                        }
                    }
                    else
                    {
                        return new JsonResult(new { status = (int)HttpStatusCode.BadRequest, message = "Record Already deleted" });
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
    }


}
