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
    /// 用户回执信息
    /// </summary>
    [Table("responds")]
    public class Respond : BaseEntityWithDate
    {
        DateTime _regTime = DateTime.Now;
        int _regState = 1;


        /// <summary>
        /// 注册码ID
        /// </summary>
        [Required]
        public int registrationID { get; set; }
        /// <summary>
        /// 注册信息
        /// </summary>
        public virtual Registration Registration { get; set; }
        /// <summary>
        /// 用户回执码
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string respondCode { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Required]
        public DateTime registrationTime
        {
            get
            {
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
    }
}
