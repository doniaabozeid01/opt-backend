using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Entities
{
    public class SupplierProducts : BaseEntity
    {
        public int Id { get; set; }
        public int SuppliersId { get; set; }
        public Suppliers Suppliers { get; set; }
        public int ProductsId { get; set; }
        public Products Products { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; }
        public DateOnly CreatedAt { get; set; }

    }

}
