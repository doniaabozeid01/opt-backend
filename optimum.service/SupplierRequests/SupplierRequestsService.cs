using optimum.data.Entities;
using optimum.repository.Interfaces;
using optimum.service.SupplierRequests.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace optimum.service.SupplierRequests
{
    public class SupplierRequestsService : ISupplierRequestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierRequestsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SupplierRequestSummaryDto>> GetSupplierRequestsAsync(int supplierId)
        {
            var repo = _unitOfWork.Repository<SupplierRequest>();

            var query = repo.GetTable()
                .Where(sr => sr.SupplierId == supplierId)
                .Include(sr => sr.SchoolRequest)
                .Include(sr => sr.Items);

            var result = await query
                .Select(sr => new SupplierRequestSummaryDto
                {
                    SupplierRequestId = sr.Id,
                    SchoolRequestId = sr.SchoolRequestId,
                    SchoolRequestCode = sr.SchoolRequest.School.Code, // غيّري للـ property الصح (ORD-001)
                    SentAt = sr.SentAt,
                    Status = sr.Status.ToString(),

                    TotalQuantity = sr.Items.Sum(i => i.Quantity),

                    Items = sr.Items.Select(i => new SupplierRequestItemDto
                    {
                        Id = i.Id,
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        Notes = i.Notes
                    }).ToList()
                })
                .OrderByDescending(x => x.SentAt)
                .ToListAsync();

            return result;
        }
    }
}
