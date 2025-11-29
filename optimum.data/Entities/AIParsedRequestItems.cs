using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Entities
{
    public class AIParsedRequestItems : BaseEntity
    {
        public int Id { get; set; }

        public int SchoolRequestId { get; set; }
        public SchoolRequests SchoolRequest { get; set; }

        public int? ProductId { get; set; }
        public Products Product { get; set; }

        public string ExtractedName { get; set; }       // الاسم اللي جه من المدرسة
        public int Quantity { get; set; }

        public double Confidence { get; set; }          // نسبة الثقة

        public string? Notes { get; set; }

        public DateTime ParsedAt { get; set; } = DateTime.UtcNow;
    }


}
