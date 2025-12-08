using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace optimum.service.SupplierOffer.Dtos
{
    public class SupplierFileOfferCreateDto
    {
        public IFormFile File { get; set; }
    }

}
