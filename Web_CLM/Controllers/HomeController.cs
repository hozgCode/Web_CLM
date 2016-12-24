using CLM.Common;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_CLM.Models;

namespace Web_CLM.Controllers
{
    public class HomeController : Controller
    {
        CLMContext db = new CLMContext();

        #region 主页
        public ActionResult Index()
        {
            return Redirect("/User/UserList");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #endregion 主页

        #region 注册
        // GET: /Home/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.ErrMsg = "";
            return View();
        }

        // POST: /Home/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                string password = model.Password.Trim();
                string md5Pwd = MD5Encode.getMd5Hash(password);
                User um = new Model.User
                {
                    userName = model.UserName.Trim(),
                    password = md5Pwd,
                    isAdmin = model.IsAdmin,
                    lastLoginTime = DateTime.Now
                };
                db.Users.Add(um);
                int addscu = db.SaveChanges();
                if (addscu > 0)
                {
                    await SignInAsync(um, false);
                    string returnUrl = "~/Home/Index";
                    return Redirect(returnUrl);
                }
                else
                {
                    ViewBag.ErrMsg = "用户注册失败！";
                }
            }
            else
            {
                ViewBag.ErrMsg = "注册信息有问题，验证不通过！";
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }
        #endregion 注册

        #region 登录
        // GET: /Home/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.ErrMsg = "";
            return View();
        }

        // POST: /Home/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string userName = model.UserName.Trim();
                string password = model.Password.Trim();
                string md5Pwd = MD5Encode.getMd5Hash(password);
                var um = db.Users.FirstOrDefault(u => u.userID == userName && u.password == md5Pwd);
                if (um != null)
                {
                    um.lastLoginTime = DateTime.Now;
                    db.SaveChanges();
                    await SignInAsync(um, false);
                    string returnUrl = "~/User/UserList";
                    return Redirect(returnUrl);
                }
                else
                {
                    ViewBag.ErrMsg = "用户名或密码错误！";
                }
            }
            else
            {
                ViewBag.ErrMsg = "登录信息有问题，验证不通过！";
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        // POST: /Home/LogOff
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Home");
        }
        #endregion 登录

        #region 用户登录状态保存
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            Claim Name = new Claim(ClaimTypes.Name, user.userName);
            Claim NameIdentifier = new Claim(ClaimTypes.NameIdentifier, user.ID.ToString());
            identity.AddClaim(Name);
            identity.AddClaim(NameIdentifier);
            Claim identityprovider = new Claim(
                "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                "ASP.NET Identity");
            identity.AddClaim(identityprovider);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
            //Response.Cookies.Add(new HttpCookie("uid", user.Id.ToString()));
        }
        #endregion 用户登录状态保存
    }
}