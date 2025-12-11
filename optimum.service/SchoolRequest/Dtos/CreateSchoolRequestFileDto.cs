using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using optimum.data.Enum;

namespace optimum.service.SchoolRequest.Dtos
{
    public class CreateSchoolRequestFileDto
    {
        public int SchoolId { get; set; }

        // لازم تكون File
        //public RequestTypeEnum RequestType { get; set; } = RequestTypeEnum.File;

        public IFormFile File { get; set; }  // PDF / Word / Excel
    }

}
