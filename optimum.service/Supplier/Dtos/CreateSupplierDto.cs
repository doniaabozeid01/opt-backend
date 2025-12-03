using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimum.data.Entities;

namespace optimum.service.Supplier.Dtos
{
    // Dtos/Suppliers/CreateSupplierDto.cs
    public class CreateSupplierDto
    {
        public string FullName { get; set; }
        //public string Code { get; set; }
        //public string UserId { get; set; }         // FK
        public string ResponsiblePerson { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
    }
}
