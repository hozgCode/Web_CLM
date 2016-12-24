using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 注册信息
    /// </summary>
    [Table("registrations")]
    public class Registration : BaseEntityWithDate
    {
        DateTime _failTime = DateTime.Now.Date.AddDays(1);
        DateTime _regTime = DateTime.Now;
        int _regState = 1;

        /// <summary>
        /// 客户ID
        /// </summary>
        [Required]
        public int customerID { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// 注册码
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string registrationKey { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Required]
        public DateTime registrationTime
        {
            get
            {
                if (_regTime == DateTime.MinValue)
                {
                    _regTime = DateTime.Now;
                }
                return _regTime;
            }
            set
            {
                _regTime = value;
            }
        }
        /// <summary>
        /// 生效状态(1=有效；0=失效)
        /// </summary>
        [Required]
        public int registrationState
        {
            get
            {
                return _regState;
            }
            set
            {
                _regState = value;
            }
        }
        /// <summary>
        /// 失效时间
        /// </summary>
        [Required]
        public DateTime failTime
        {
            get
            {
                return _failTime;
            }
            set
            {
                _failTime = value;
            }
        }
        /// <summary>
        /// 失效原因
        /// </summary>
        [MaxLength(200)]
        public string failReason { get; set; }
        /// <summary>
        /// 购买客户端数量
        /// </summary>
        [Required]
        public int licenceCount { get; set; }
        /// <summary>
        /// 已注册使用的数量
        /// </summary>
        [Required]
        public int usedCount { get; set; }
    }
}
