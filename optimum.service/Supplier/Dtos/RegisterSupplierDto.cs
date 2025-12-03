using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.Supplier.Dtos
{
    public class RegisterSupplierDto
    {
        //public string FullName { get; set; }
        //public string Code { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }



        public string FullName { get; set; }
        public string Code { get; set; }

        // دول هيروحوا لـ Identity
        public string Email { get; set; }
        public string Password { get; set; }

        // بيانات إضافية للمورد لو حابة
        public string ResponsiblePerson { get; set; }
        public string Phone { get; set; }

        // المنتجات اللي بيبيعها
        public List<RegisterSupplierProductDto> Products { get; set; }

    }
}
