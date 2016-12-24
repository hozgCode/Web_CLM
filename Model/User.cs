using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Users")]
    public class User : BaseEntity
    {
        int _dataState = 1;
        DateTime _loginTime = DateTime.Now;
        DateTime _insertTime = DateTime.Now;
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string userID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string userName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string password { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        [Required]
        public bool isAdmin { get; set; }

        /// <summary>
        /// 建号时间
        /// </summary>
        [Required]
        public DateTime insertTime
        {
            get
            {
                if (_insertTime == DateTime.MinValue)
                {
                    _insertTime = DateTime.Now;
                }
                return _insertTime;
            }
            set
            {
                _insertTime = value;
            }
        }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        [Required]
        public DateTime lastLoginTime
        {
            get
            {
                if (_loginTime == DateTime.MinValue)
                {
                    _loginTime = DateTime.Now;
                }
                return _loginTime;
            }
            set
            {
                _loginTime = value;
            }
        }
        /// <summary>
        /// 状态(1=正常；0=停用)
        /// </summary>
        [Required]
        public int dataState
        {
            get { return _dataState; }
            set { _dataState = value; }
        }
    }
}
