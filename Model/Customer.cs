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
    /// 客户信息
    /// </summary>
    [Table("customers")]
    public class Customer : BaseEntityWithDate
    {
        int _dataState = 1;
        DateTime _insertTime = DateTime.Now;

        /// <summary>
        /// 客户编号
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string customerNumber { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string customerName { get; set; }
        /// <summary>
        /// 客户地址
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string customerAddress { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string customerContact { get; set; }
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
        /// 总购买客户端数量
        /// </summary>
        [Required]
        public int licenceCount { get; set; }
        /// <summary>
        /// 总已注册使用的数量
        /// </summary>
        [Required]
        public int usedCount { get; set; }
        /// <summary>
        /// 状态(1=正常；0=停用)
        /// </summary>
        [Required]
        public int dataState
        {
            get { return _dataState; }
            set { _dataState = value; }
        }
        /// <summary>
        /// 是否进行license管理
        /// </summary>
        [Required]
        public bool islicense { get; set; }
    }
}
