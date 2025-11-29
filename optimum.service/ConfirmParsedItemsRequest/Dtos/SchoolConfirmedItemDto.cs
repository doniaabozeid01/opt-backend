using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.ConfirmParsedItemsRequest.Dtos
{
    public class SchoolConfirmedItemDto
    {
        public int Id { get; set; }
        public int SchoolRequestId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public bool IsConfirmed { get; set; }
    }

}
