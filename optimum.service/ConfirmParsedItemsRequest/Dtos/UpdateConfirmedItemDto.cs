using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.ConfirmParsedItemsRequest.Dtos
{
    public class UpdateConfirmedItemDto
    {
        public int Id { get; set; }              // Id من جدول SchoolConfirmedRequestItems
        public int? ProductId { get; set; }      // المدرسة تختاره من Dropdown
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public bool IsConfirmed { get; set; }    // المدرسة تعمل Check على اللي وافقت عليه
    }

}
