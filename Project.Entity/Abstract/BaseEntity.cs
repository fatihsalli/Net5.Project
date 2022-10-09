using Project.Entity.Enum;
using Project.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Abstract
{
    public class BaseEntity:IEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
            Status = Enum.DataStatus.Inserted;
        }
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }
        public bool IsActive { get; set; }
    }
}
