using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Core.Exceptions
{
    public class UnAuthorizedActionException:Exception
    {
        public UnAuthorizedActionException()
        {

        }
        public UnAuthorizedActionException(string message)
            :base(message)
        {
                
        }
    }
}
