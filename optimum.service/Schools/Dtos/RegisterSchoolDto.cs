using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.Schools.Dtos
{
    public class RegisterSchoolDto
    {
        public string FullName { get; set; }
        public string Code { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        // من الـ Modal (بيانات المدرسة)
        public string ResponsiblePerson { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }      // المدينة، الحي
    }
}
