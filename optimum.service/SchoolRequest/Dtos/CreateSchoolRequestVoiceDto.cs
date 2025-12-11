using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using optimum.data.Enum;

namespace optimum.service.SchoolRequest.Dtos
{
    public class CreateSchoolRequestVoiceDto
    {
        public int SchoolId { get; set; }

        // public RequestTypeEnum RequestType { get; set; } = RequestTypeEnum.Voice;

        public IFormFile Audio { get; set; } // mp3 / wav
    }

}
