using optimum.data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Entities
{
    //public class SupplierOffers : BaseEntity
    //{
    //    public int Id { get; set; }
    //    public int SupplierRequestId { get; set; }
    //    public SupplierRequest SupplierRequest { get; set; }
    //    public DateTime OfferDate { get; set; } = DateTime.UtcNow;
    //    public string QuoteNumber { get; set; }   // QUO-001
    //    public int DeliveryDays { get; set; }     // مدة التوصيل
    //    public decimal PurchaseCost { get; set; } // اجمالي تكلفة الشراء (اختياري snapshot)
    //    public decimal FinalPrice { get; set; }   // السعر النهائي للمدرسة
    //    public string SupplierNotes { get; set; }
    //    public string Status { get; set; } = "Pending";
    //    // Pending – Submitted – ApprovedByAdmin – ApprovedBySchool – Rejected
    //    public ICollection<SupplierOfferItems> Items { get; set; }
    //}




    public class SupplierOffers : BaseEntity
    {
        public int Id { get; set; }
        public int SupplierRequestId { get; set; }
        public SupplierRequest SupplierRequest { get; set; }

        // نوع العرض
        public SupplierOfferType OfferType { get; set; }

        // الداتا الأصلية من المورد
        public string RawText { get; set; }       // لو Free Text
        public string FilePath { get; set; }      // لو PDF / Voice
        public string FileName { get; set; }
        public string ContentType { get; set; }

        // الداتا النهائية بعد تأكيد المورد
        public decimal? PurchaseCost { get; set; }
        public decimal? ProfitMarginPercent { get; set; } = 10;
        public decimal? ProfitMarginValue { get; set; }
        public decimal? FinalPrice { get; set; }

        public int? DeliveryDays { get; set; }
        public string SupplierNotes { get; set; }

        // حالة العرض
        public SupplierOfferStatus Status { get; set; } = SupplierOfferStatus.PendingAI;

        // تأكيد المورد
        public bool IsConfirmedBySupplier { get; set; }
        public DateTime? SupplierConfirmedAt { get; set; }

        // Date
        public DateTime OfferDate { get; set; } = DateTime.UtcNow;
        public string QuoteNumber { get; set; }

        // البنود
        public ICollection<SupplierOfferItems> Items { get; set; }
    }

}
