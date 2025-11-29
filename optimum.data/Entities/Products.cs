using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Entities
{
    public class Products : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }                // الاسم الأساسي للمنتج
        public string Description { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        public string Keywords { get; set; }            // قائمة كلمات مفتاحية (JSON or CSV)
    }

}
