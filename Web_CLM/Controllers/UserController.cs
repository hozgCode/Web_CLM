using CLM.Common;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_CLM.Models;

namespace Web_CLM.Controllers
{
    public class UserController : Controller
    {
        CLMContext db = new CLMContext();

        // GET: /User/UserList
        public ActionResult UserList()
        {
            int uCount = db.Users.Where(u => u.dataState == 1).Count();
            ViewBag.TotalCount = uCount;
            return View();
        }

        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserListByPage()
        {
            int rowCount = Convert.ToInt32(Request.Params["rowCount"]);
            int pageSize = Convert.ToInt32(Request.Params["pageSize"]);
            var userlist = db.Users.Where(u => u.dataState == 1).OrderBy(u => u.ID).Skip(rowCount).Take(pageSize).ToList();
            var reslist = from u in userlist
                          select new
                          {
                              u.ID,
                              u.userID,
                              u.userName,
                              isAdmin = u.isAdmin,
                              lastLoginTime = u.lastLoginTime.ToString("yyyy-MM-dd HH:mm:ss")
                          };
            return Json(reslist, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> CheckUser()
        {
            string msg = "ok";
            string uid = Request.Params["UserID"].Trim();
            var olduser = db.Users.FirstOrDefault(u => u.userID == uid);
            if (olduser != null)
            {
                msg = "该用户帐号已经被注册过！";
            }
            return msg;
        }

        //post:/User/AddUser
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddUser(RegisterModel model)
        {
            bool suc = false;
            string errmsg = "";
            if (ModelState.IsValid)
            {
                string password = model.Password.Trim();
                string md5Pwd = MD5Encode.getMd5Hash(password);
                User um = new Model.User
                {
                    userID = model.UserID.Trim(),
                    userName = model.UserName.Trim(),
                    password = md5Pwd,
                    isAdmin = false,
                    lastLoginTime = DateTime.Now
                };
                db.Users.Add(um);
                int addscu = db.SaveChanges();
                if (addscu > 0)
                {
                    suc = true;
                }
                else
                {
                    errmsg = "用户添加时出现问题！";
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
        /// 更新用户数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> UpdateUser(User model)
        {
            var olduser = db.Users.FirstOrDefault(u => u.ID == model.ID);
            if (olduser != null)
            {
                olduser.userID = model.userID;
                olduser.userName = model.userName;
                olduser.isAdmin = model.isAdmin;
            }
            int upsuc = db.SaveChanges();
            return upsuc;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteUser()
        {
            string delIdString = Request.Params["delIds"];
            List<int> delIds = JsonConvert.DeserializeObject<List<int>>(delIdString);
            var delUsers = db.Users.Where(u => delIds.Contains(u.ID)).ToList();
            foreach (var item in delUsers)
            {
                item.dataState = 0;
            }
            int delsuc = db.SaveChanges();
            return delsuc;
        }
    }
}