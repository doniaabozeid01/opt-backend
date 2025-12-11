using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SupplierOfferParser.Dtos
{
    public class ParsedSupplierOfferDto
    {
        public decimal? SuggestedPurchaseCost { get; set; }
        public int? SuggestedDeliveryDays { get; set; }
        public string SuggestedNotes { get; set; }

        // سعر لكل بند في SupplierRequest.Items بالترتيب
        public List<decimal> UnitPrices { get; set; } = new();

        // لو حبيتي تحطي JSON من AI بعدين
        public string RawJson { get; set; }
    }
}
