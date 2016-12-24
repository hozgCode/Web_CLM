using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BaseEntityWithDate : BaseEntity
    {
        private bool _deleteMark = false;
        private DateTime _deleteTime;
        private DateTime _updatedDate;
        private DateTime _createdDate;

        [Required]
        public bool deleteMark
        {
            get { return _deleteMark; }
            set { _deleteMark = value; }
        }
        public int? deleteUser { get; set; }
        public DateTime? deleteTime { get; set; }
        [Required]
        public int creatUser { get; set; }
        public DateTime creatTime
        {
            get
            {
                if (_createdDate == DateTime.MinValue)
                {
                    _createdDate = DateTime.Now;
                }
                return _createdDate;
            }
            set
            {
                _createdDate = value;
            }
        }
        public int upDateUser { get; set; }
        public DateTime upDateTime
        {
            get
            {
                if (_updatedDate == DateTime.MinValue)
                {
                    _updatedDate = DateTime.Now;
                }
                return _updatedDate;
            }
            set
            {
                _updatedDate = value;
            }
        }

    }
}
