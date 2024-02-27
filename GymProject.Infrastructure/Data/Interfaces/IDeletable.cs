using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Interfaces
{
    public interface IDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime DeleteTime { get; set; }
    }
}
