using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.Supplier.Dtos
{
    // Dtos/Suppliers/CreateSupplierDto.cs
    public class CreateSupplierDto
    {
        public string SupplierName { get; set; }
        public string ResponsiblePerson { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
    }
}
