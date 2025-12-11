using Microsoft.EntityFrameworkCore;
using optimum.data.Context;
using optimum.data.Enum;
using optimum.service.SchoolRequest.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SchoolRequest
{
    public class SchoolRequestService : ISchoolRequestService
    {
        readonly OptimumDbContext _context;
        public SchoolRequestService(OptimumDbContext context)
        {
            _context = context;
        }
        public async Task<SchoolRequestsStatsDto> GetSchoolRequestsStatsAsync(int schoolId)
        {
            var query = _context.SchoolRequests
                .Where(r => r.SchoolId == schoolId);

            var result = new SchoolRequestsStatsDto
            {
                Total = await query.CountAsync(),
                Pending = await query.CountAsync(r => r.Status == RequestStatusEnum.Pending),
                InProgress = await query.CountAsync(r => r.Status == RequestStatusEnum.InProgress),
                Completed = await query.CountAsync(r => r.Status == RequestStatusEnum.Completed),
            };

            return result;
        }

    }
}
