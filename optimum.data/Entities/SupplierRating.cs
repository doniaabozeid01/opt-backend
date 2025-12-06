using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Entities
{
    public class SupplierRating : BaseEntity
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }
        public Suppliers Supplier { get; set; }

        public int? SupplierRequestId { get; set; }   // الطلب اللي اتقيّم عليه
        public SupplierRequest SupplierRequest { get; set; }

        public int? SupplierOfferId { get; set; }     // العرض لو حابة تربطيه
        public SupplierOffers SupplierOffer { get; set; }

        public decimal Score { get; set; }            // من 0 لغاية 10
        public string Comment { get; set; }           // ملاحظات المدرسة

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
