using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Enum
{
    public enum RequestStatusEnum
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3,
        AI_Analyzed = 4,
        Confirmed = 5
    }
}