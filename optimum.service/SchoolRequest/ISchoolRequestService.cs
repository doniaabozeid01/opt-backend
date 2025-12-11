using optimum.service.SchoolRequest.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SchoolRequest
{
    public interface ISchoolRequestService
    {
        Task<SchoolRequestsStatsDto> GetSchoolRequestsStatsAsync(int schoolId);

    }
}
