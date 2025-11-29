using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimum.data.Enum;

namespace optimum.service.SchoolRequest.Dtos
{
    public class CreateSchoolRequestDto
    {
        public int SchoolId { get; set; }

        // Text أو Form فقط هنا
        public RequestTypeEnum RequestType { get; set; }

        // لو نوع الطلب Text
        //public string TextContent { get; set; }

        // لو نوع الطلب Form
        public List<SchoolRequestItemDto> Items { get; set; }
    }
}
