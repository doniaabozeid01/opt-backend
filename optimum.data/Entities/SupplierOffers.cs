using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Entities
{
    public class SupplierOffers : BaseEntity
    {
        public int Id { get; set; }

        public int SupplierRequestId { get; set; }
        public SupplierRequest SupplierRequest { get; set; }

        public DateTime OfferDate { get; set; } = DateTime.UtcNow;

        public string QuoteNumber { get; set; }   // QUO-001
        public int DeliveryDays { get; set; }     // مدة التوصيل

        public decimal PurchaseCost { get; set; } // اجمالي تكلفة الشراء (اختياري snapshot)
        public decimal FinalPrice { get; set; }   // السعر النهائي للمدرسة

        public string SupplierNotes { get; set; }

        public string Status { get; set; } = "Pending";
        // Pending – Submitted – ApprovedByAdmin – ApprovedBySchool – Rejected

        public ICollection<SupplierOfferItems> Items { get; set; }
    }

}
