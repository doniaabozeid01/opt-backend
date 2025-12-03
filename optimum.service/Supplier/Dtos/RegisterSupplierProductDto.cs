using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.Supplier.Dtos
{
    public class RegisterSupplierProductDto
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; }
    }
}
