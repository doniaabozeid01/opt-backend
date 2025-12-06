using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SupplierRate.Dtos
{
    public class SupplierRatingCreateDto
    {
        public int SupplierId { get; set; }
        public int? SupplierRequestId { get; set; }
        public int? SupplierOfferId { get; set; }

        public decimal Score { get; set; }      // من 0 ل 10
        public string Comment { get; set; }
    }

}
