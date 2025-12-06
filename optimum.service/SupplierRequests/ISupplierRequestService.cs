using optimum.service.SupplierRequests.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SupplierRequests
{
    public interface ISupplierRequestService
    {
        Task<List<SupplierRequestSummaryDto>> GetSupplierRequestsAsync (int supplierId);

    }
}
