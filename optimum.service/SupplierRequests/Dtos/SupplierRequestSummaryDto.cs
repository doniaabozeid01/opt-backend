using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SupplierRequests.Dtos
{
    public class SupplierRequestSummaryDto
    {
        public int SupplierRequestId { get; set; }

        public int SchoolRequestId { get; set; }
        public string SchoolRequestCode { get; set; }  // زي ORD-001
        public DateTime SentAt { get; set; }

        public string Status { get; set; }             // "Sent" / "Viewed" / ...

        public int TotalQuantity { get; set; }

        public List<SupplierRequestItemDto> Items { get; set; }
    }
}
