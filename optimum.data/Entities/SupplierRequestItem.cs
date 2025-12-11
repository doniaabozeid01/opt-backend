using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Entities
{
    public class SupplierRequestItem : BaseEntity
    {
        public int Id { get; set; }

        // ========== FK to SupplierRequest (الهيدر) ==========
        public int SupplierRequestId { get; set; }
        public SupplierRequest SupplierRequest { get; set; }

        // ========== FK للبند المؤكد من المدرسة ==========
        public int SchoolConfirmedRequestItemId { get; set; }
        public SchoolConfirmedRequestItems SchoolConfirmedRequestItem { get; set; }

        // ========== Snapshot للعرض ==========
        public int? ProductId { get; set; }
        public Products Product { get; set; }

        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public string Notes { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }

}
