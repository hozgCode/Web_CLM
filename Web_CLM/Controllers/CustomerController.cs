using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web_CLM.Controllers
{
    public class CustomerController : BaseController
    {
        CLMContext db = new CLMContext();

        #region 客户管理
        // GET: /Customer/CustomerManage
        public ActionResult CustomerManage()
        {
            int uCount = db.Customers.Where(u => u.deleteMark == false).Count();
            ViewBag.TotalCount = uCount;
            return View();
        }

        /// <summary>
        /// 分页获取客户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCustomerListByPage()
        {
            int rowCount = Convert.ToInt32(Request.Params["rowCount"]);
            int pageSize = Convert.ToInt32(Request.Params["pageSize"]);
            var userlist = from u in db.Customers
                           where u.deleteMark == false
                           orderby u.ID
                           select new
                           {
                               u.ID,
                               u.customerNumber,
                               u.customerName,
                               u.customerAddress,
                               u.customerContact,
                               u.licenceCount,
                               u.usedCount,
                               u.islicense
                           };
            var reslist = userlist.Skip(rowCount).Take(pageSize).ToList();
            return Json(reslist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检测客户编号是否注册过
        /// </summary>
        /// <returns></returns>
        public async Task<string> CheckCustomer()
        {
            string msg = "ok";
            string uid = Request.Params["uid"].Trim();
            var olduser = db.Customers.FirstOrDefault(u => u.customerNumber == uid);
            if (olduser != null)
            {
                msg = "该客户编号已经被注册过！";
            }
            return msg;
        }

        //post:/Customer/AddCustomer
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddCustomer(Customer model)
        {
            bool suc = false;
            string errmsg = "";
            if (ModelState.IsValid)
            {
                model.insertTime = DateTime.Now;
                model.dataState = 1;
                ModelCreate(model);
                db.Customers.Add(model);
                int addscu = db.SaveChanges();
                if (addscu > 0)
                {
                    suc = true;
                }
                else
                {
                    errmsg = "客户添加时出现问题！";
                }
            }
            else
            {
                errmsg = "客户信息有问题，验证不通过！";
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return Json(new { suc, errmsg }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更新客户数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> UpdateCustomer(Customer model)
        {
            var olduser = db.Customers.FirstOrDefault(u => u.ID == model.ID);
            if (olduser != null)
            {
                olduser.customerNumber = model.customerNumber;
                olduser.customerName = model.customerName;
                olduser.customerAddress = model.customerAddress;
                olduser.customerContact = model.customerContact;
                olduser.usedCount = model.usedCount;
                olduser.licenceCount = model.licenceCount;
                olduser.islicense = model.islicense;
                ModelUpdate(olduser);
            }
            int upsuc = db.SaveChanges();
            return upsuc;
        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteCustomer()
        {
            string delIdString = Request.Params["delIds"];
            List<int> delIds = JsonConvert.DeserializeObject<List<int>>(delIdString);
            var delUsers = db.Customers.Where(u => delIds.Contains(u.ID)).ToList();
            foreach (var item in delUsers)
            {
                item.dataState = 0;
                ModelDelete(item);
            }
            int delsuc = db.SaveChanges();
            return delsuc;
        }
        #endregion 客户管理

        #region 注册信息管理
        // GET: /Customer/RegistrationManage
        public ActionResult RegistrationManage()
        {
            int uCount = db.Registrations.Where(u => u.deleteMark == false).Count();
            ViewBag.TotalCount = uCount;
            return View();
        }

        /// <summary>
        /// 分页获取注册信息列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRegistrationListByPage()
        {
            int rowCount = Convert.ToInt32(Request.Params["rowCount"]);
            int pageSize = Convert.ToInt32(Request.Params["pageSize"]);
            var userlist = from u in db.Registrations
                           where u.deleteMark == false
                           orderby u.ID
                           select new
                           {
                               u.ID,
                               u.customerID,
                               u.Customer.customerNumber,
                               u.registrationKey,
                               u.registrationTime,
                               u.registrationState,
                               u.failTime,
                               u.failReason,
                               u.licenceCount,
                               u.usedCount
                           };
            var restemp = userlist.Skip(rowCount).Take(pageSize).ToList();
            var reslist = from u in restemp
                          select new
                          {
                              u.ID,
                              u.customerID,
                              u.customerNumber,
                              u.registrationKey,
                              registrationTime = u.registrationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                              u.registrationState,
                              failTime = u.failTime.ToString("yyyy-MM-dd"),
                              failReason = String.IsNullOrWhiteSpace(u.failReason) ? "" : u.failReason,
                              u.licenceCount,
                              u.usedCount
                          };
            return Json(reslist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取可用的客户编号
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCustomerItems()
        {
            var ctlist = (from ct in db.Customers
                          where ct.dataState == 1 && ct.deleteMark == false && ct.licenceCount > ct.usedCount
                          select new
                          {
                              ct.ID,
                              ct.customerNumber
                          }).ToList();
            return Json(ctlist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检测注册码是否注册过
        /// </summary>
        /// <returns></returns>
        public async Task<string> CheckRegistration()
        {
            string msg = "ok";
            string uid = Request.Params["uid"].Trim();
            var olduser = db.Registrations.FirstOrDefault(u => u.registrationKey == uid);
            if (olduser != null)
            {
                msg = "该注册码已经被注册过！";
            }
            return msg;
        }

        //post:/Customer/AddCustomer
        /// <summary>
        /// 添加注册信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddRegistration(Registration model)
        {
            bool suc = false;
            string errmsg = "";
            if (ModelState.IsValid)
            {
                ModelCreate(model);
                db.Registrations.Add(model);
                int addscu = db.SaveChanges();
                if (addscu > 0)
                {
                    suc = true;
                }
                else
                {
                    errmsg = "注册信息添加时出现问题！";
                }
            }
            else
            {
                errmsg = "注册信息有问题，验证不通过！";
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return Json(new { suc, errmsg }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更新注册信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> UpdateRegistration(Registration model)
        {
            var olduser = db.Registrations.FirstOrDefault(u => u.ID == model.ID);
            if (olduser != null)
            {
                olduser.registrationState = model.registrationState;
                olduser.failTime = model.failTime;
                olduser.failReason = model.failReason;
                olduser.usedCount = model.usedCount;
                olduser.licenceCount = model.licenceCount;
                ModelUpdate(olduser);
            }
            int upsuc = db.SaveChanges();
            return upsuc;
        }

        /// <summary>
        /// 删除注册信息
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteRegistration()
        {
            string delIdString = Request.Params["delIds"];
            List<int> delIds = JsonConvert.DeserializeObject<List<int>>(delIdString);
            var delUsers = db.Registrations.Where(u => delIds.Contains(u.ID)).ToList();
            foreach (var item in delUsers)
            {
                item.registrationState = 0;
                item.failTime = DateTime.Now;
                ModelDelete(item);
            }
            int delsuc = db.SaveChanges();
            return delsuc;
        }
        #endregion 注册信息管理

        #region 回执信息管理
        // GET: /Customer/RespondManage
        public ActionResult RespondManage()
        {
            int uCount = db.Responds.Where(u => u.deleteMark == false).Count();
            ViewBag.TotalCount = uCount;
            return View();
        }

        /// <summary>
        /// 分页获取回执信息列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRespondListByPage()
        {
            int rowCount = Convert.ToInt32(Request.Params["rowCount"]);
            int pageSize = Convert.ToInt32(Request.Params["pageSize"]);
            var userlist = from u in db.Responds
                           where u.deleteMark == false
                           orderby u.ID
                           select new
                           {
                               u.ID,
                               u.registrationID,
                               u.Registration.registrationKey,
                               u.respondCode,
                               u.registrationTime,
                               u.registrationState
                           };
            var restemp = userlist.Skip(rowCount).Take(pageSize).ToList();
            var reslist = from u in restemp
                          select new
                          {
                              u.ID,
                              u.registrationID,
                              u.registrationKey,
                              u.respondCode,
                              registrationTime = u.registrationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                              u.registrationState
                          };
            return Json(reslist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取可用的注册码
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRegkeys()
        {
            var ctlist = (from ct in db.Registrations
                          where ct.deleteMark == false && ct.licenceCount > ct.usedCount
                          select new
                          {
                              ct.ID,
                              ct.registrationKey
                          }).ToList();
            return Json(ctlist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检测回执码是否注册过
        /// </summary>
        /// <returns></returns>
        public async Task<string> CheckRespond()
        {
            string msg = "ok";
            string uid = Request.Params["uid"].Trim();
            var olduser = db.Responds.FirstOrDefault(u => u.respondCode == uid);
            if (olduser != null)
            {
                msg = "该设备回执码已经被注册过！";
            }
            return msg;
        }

        //post:/Customer/AddCustomer
        /// <summary>
        /// 添加回执信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddRespond(Respond model)
        {
            bool suc = false;
            string errmsg = "";
            if (ModelState.IsValid)
            {
                ModelCreate(model);
                db.Responds.Add(model);
                int addscu = db.SaveChanges();
                if (addscu > 0)
                {
                    suc = true;
                }
                else
                {
                    errmsg = "回执信息添加时出现问题！";
                }
            }
            else
            {
                errmsg = "回执信息有问题，验证不通过！";
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return Json(new { suc, errmsg }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更新回执信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> UpdateRespond(Respond model)
        {
            var olduser = db.Responds.FirstOrDefault(u => u.ID == model.ID);
            if (olduser != null)
            {
                olduser.registrationState = model.registrationState;
                ModelUpdate(olduser);
            }
            int upsuc = db.SaveChanges();
            return upsuc;
        }

        /// <summary>
        /// 删除回执信息
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteRespond()
        {
            string delIdString = Request.Params["delIds"];
            List<int> delIds = JsonConvert.DeserializeObject<List<int>>(delIdString);
            var delUsers = db.Responds.Where(u => delIds.Contains(u.ID)).ToList();
            foreach (var item in delUsers)
            {
                item.registrationState = 0;
                ModelDelete(item);
            }
            int delsuc = db.SaveChanges();
            return delsuc;
        }
        #endregion 回执信息管理
    }
}