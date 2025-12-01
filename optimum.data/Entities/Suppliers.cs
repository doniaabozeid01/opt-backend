using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Entities
{
    public class Suppliers : BaseEntity
    {
        public int Id { get; set; }
        //public string SupplierName { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }         // FK
        public ApplicationUser User { get; set; }  // Navigation
        public string ResponsiblePerson { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public DateOnly CreatedAt { get; set; }

    }
}
