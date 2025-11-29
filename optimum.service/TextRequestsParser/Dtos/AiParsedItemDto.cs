using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.TextRequestsParser.Dtos
{
    public class AiParsedItemDto
    {
        public int? ProductId { get; set; }   // مهم جدًا
        public string ExtractedName { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
    }
}
