using optimum.data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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

        // ========== الربط مع الطلب الخاص بالمورد ==========

        public int SupplierRequestId { get; set; }
        [JsonIgnore]              // 👈
        public SupplierRequest SupplierRequest { get; set; } = null!;

        // ========== نوع العرض ==========

        public SupplierOfferType OfferType { get; set; }

        // ========== الداتا الخام من المورد ==========

        // لو العرض FreeText هنستخدم RawText
        public string? RawText { get; set; }

        // لو العرض ملف (PDF / Word / Audio) هنستخدم دول
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }

        // ========== الداتا النهائية بعد تأكيد المورد ==========

        public decimal? PurchaseCost { get; set; }          // إجمالي سعر المورد
        public decimal ProfitMarginPercent { get; set; } = 10m; // دايماً 10%
        public decimal? ProfitMarginValue { get; set; }     // PurchaseCost * 10%
        public decimal? FinalPrice { get; set; }            // PurchaseCost + ProfitMarginValue

        public int? DeliveryDays { get; set; }              // أيام التسليم
        public string? SupplierNotes { get; set; }          // ملاحظات المورد (اختياري)

        // ========== حالة العرض ==========

        public SupplierOfferStatus Status { get; set; } = SupplierOfferStatus.PendingAI;

        public bool IsConfirmedBySupplier { get; set; }
        public DateTime? SupplierConfirmedAt { get; set; }

        // ========== بيانات إضافية ==========

        public DateTime OfferDate { get; set; } = DateTime.UtcNow;
        public string? QuoteNumber { get; set; }            // QUO-001 مثلاً (اختياري في الأول)

        // ========== البنود (لو هتفصّلي كل منتج في العرض) ==========

        public ICollection<SupplierOfferItems> Items { get; set; } = new List<SupplierOfferItems>();
    }









    //{
    //    public int Id { get; set; }
    //    public int SupplierRequestId { get; set; }
    //    public SupplierRequest SupplierRequest { get; set; }

    //    // نوع العرض
    //    public SupplierOfferType OfferType { get; set; }

    //    // الداتا الأصلية من المورد
    //    public string RawText { get; set; }       // لو Free Text
    //    public string FilePath { get; set; }      // لو PDF / Voice
    //    public string FileName { get; set; }
    //    public string ContentType { get; set; }

    //    // الداتا النهائية بعد تأكيد المورد
    //    public decimal? PurchaseCost { get; set; }
    //    public decimal? ProfitMarginPercent { get; set; } = 10;
    //    public decimal? ProfitMarginValue { get; set; }
    //    public decimal? FinalPrice { get; set; }

    //    public int? DeliveryDays { get; set; }
    //    public string SupplierNotes { get; set; }

    //    // حالة العرض
    //    public SupplierOfferStatus Status { get; set; } = SupplierOfferStatus.PendingAI;

    //    // تأكيد المورد
    //    public bool IsConfirmedBySupplier { get; set; }
    //    public DateTime? SupplierConfirmedAt { get; set; }

    //    // Date
    //    public DateTime OfferDate { get; set; } = DateTime.UtcNow;
    //    public string QuoteNumber { get; set; }

    //    // البنود
    //    public ICollection<SupplierOfferItems> Items { get; set; }
    //}

}
