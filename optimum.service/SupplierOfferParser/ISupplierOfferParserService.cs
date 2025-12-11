using optimum.data.Entities;
using optimum.service.SupplierOfferParser.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SupplierOfferParser
{
    public interface ISupplierOfferParserService
    {
        Task<ParsedSupplierOfferDto> ParseAsync(SupplierOffers offer, SupplierRequest supplierRequest);
    }
}
