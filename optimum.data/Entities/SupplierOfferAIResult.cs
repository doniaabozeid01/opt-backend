using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Entities
{
    public class SupplierOfferAIResult : BaseEntity
    {
        public int Id { get; set; }

        public int SupplierOfferId { get; set; }
        public SupplierOffers SupplierOffer { get; set; }

        public decimal? SuggestedPurchaseCost { get; set; }
        public int? SuggestedDeliveryDays { get; set; }
        public string SuggestedNotes { get; set; }

        public string RawExtractionJson { get; set; } // لو حابة تشيليها برضه عادي
    }
}
