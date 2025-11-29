using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimum.data.Enum;

namespace optimum.data.Entities
{
    public class SupplierRequest : BaseEntity
    {
        public int Id { get; set; }

        public int SchoolRequestId { get; set; }
        public SchoolRequests SchoolRequest { get; set; }

        public int SupplierId { get; set; }
        public Suppliers Supplier { get; set; }

        // حالة الطلب عند المورد
        public SupplierRequestStatus Status { get; set; } = SupplierRequestStatus.Sent;
        // Sent – Viewed – Offered – Rejected – Expired

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? ViewedAt { get; set; }
        public DateTime? LastOfferAt { get; set; }
        public SupplierOffers Offer { get; set; }


        // ممكن supplier يعمل أكتر من عرض على نفس الطلب (لو حابة تسمحي بده)
        //public ICollection<SupplierOffers> Offers { get; set; }
    }


}
