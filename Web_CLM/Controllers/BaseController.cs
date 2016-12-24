using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Web_CLM.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <param name="model"></param>
        protected void ModelCreate(BaseEntityWithDate model)
        {
            int uid = Convert.ToInt32(User.Identity.GetUserId());
            DateTime dnow = DateTime.Now;
            model.creatTime = dnow;
            model.creatUser = uid;
            model.upDateTime = dnow;
            model.upDateUser = uid;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        protected void ModelUpdate(BaseEntityWithDate model)
        {
            int uid = Convert.ToInt32(User.Identity.GetUserId());
            DateTime dnow = DateTime.Now;
            model.upDateTime = dnow;
            model.upDateUser = uid;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model"></param>
        protected void ModelDelete(BaseEntityWithDate model)
        {
            int uid = Convert.ToInt32(User.Identity.GetUserId());
            DateTime dnow = DateTime.Now;
            model.upDateTime = dnow;
            model.upDateUser = uid;
            model.deleteMark = true;
            model.deleteTime = dnow;
            model.deleteUser = uid;
        }
    }
}