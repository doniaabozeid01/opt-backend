using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.Supplier.Dtos
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string ResponsiblePerson { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
