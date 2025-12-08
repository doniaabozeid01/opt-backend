using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimum.data.Enum;
using optimum.data.Entities;
using optimum.service.SupplierOffer.Dtos;

namespace optimum.service.SupplierOffer
{
    public interface ISupplierOfferService
    {
        Task<SupplierOffers> SubmitFreeTextOfferAsync(int supplierRequestId, int supplierId, SupplierFreeTextOfferCreateDto dto);
        Task<SupplierOffers> SubmitFileOrVoiceOfferAsync(int supplierRequestId, int supplierId, SupplierFileOfferCreateDto dto, SupplierOfferType type);
    }
}
