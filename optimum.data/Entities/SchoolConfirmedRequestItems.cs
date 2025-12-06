using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Entities
{
    public class SchoolConfirmedRequestItems : BaseEntity
    {
        public int Id { get; set; }
        public int SchoolRequestId { get; set; }
        public SchoolRequests SchoolRequest { get; set; }
        // المنتج الحقيقي اللي المدرسة اختارته من اللستة
        public int ProductId { get; set; }
        public Products Product { get; set; }
        // اسم للعرض (ممكن يكون نفس اسم الـ Product أو اسم مخصص)
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public DateTime ConfirmedAt { get; set; } = DateTime.UtcNow;
        // رابط اختياري للـ AI item اللي جه منه البند ده (للتتبع بس)
        public int? AIParsedItemId { get; set; }
        public AIParsedRequestItems AIParsedItem { get; set; }
    }

}