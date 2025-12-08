using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SupplierOffer.Dtos
{
    public class SupplierFreeTextOfferCreateDto
    {
        public int SupplierId { get; set; }   // 👈 هنستقبل SupplierId من الواجهة

        public string RawText { get; set; }
    }

}
