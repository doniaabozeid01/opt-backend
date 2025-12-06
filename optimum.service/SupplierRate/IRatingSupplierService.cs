using optimum.service.SupplierRate.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SupplierRate
{
    public interface IRatingSupplierService
    {
        Task RateSupplierAsync(SupplierRatingCreateDto dto);
    }
}
