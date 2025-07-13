using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPX_Pos
{
    public interface IPOS
    {
        string DisplayWelcomeScreen();
        Guid CreateCustomerOrder();

    }

}
