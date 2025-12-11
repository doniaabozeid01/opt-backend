using optimum.data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SchoolRequest.Dtos
{
    public class CreateSchoolRequestFreeTxtDto
    {
        public int SchoolId { get; set; }

        // Text أو Form فقط هنا
        // public RequestTypeEnum RequestType { get; set; }

        // لو نوع الطلب Text
        public string TextContent { get; set; }
    }
}
