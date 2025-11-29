using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimum.data.Enum;

namespace optimum.data.Entities
{
    public class SchoolRequests : BaseEntity
    {
        public int Id { get; set; }

        // ---- علاقات المدرسة ----
        public int SchoolId { get; set; }
        public School School { get; set; }

        // ---- نوع الطلب (Voice, Text, File, Form) ----
        public RequestTypeEnum RequestType { get; set; }

        // ---- محتوى الطلب الخام ----
        public string? TextContent { get; set; }           // لو فري تكست
        public string? FilePath { get; set; }              // لو رفع ملف PDF/Word/Excel
        public string? AudioPath { get; set; }             // لو Voice Record

        // ---- حالة الطلب ----
        public string Status { get; set; } = "Pending";   // Pending – AI_Analyzed – Confirmed – Completed

        // ---- من اللي رفع الطلب ----
        //public string CreatedByUserId { get; set; }
        //public ApplicationUser CreatedByUser { get; set; }

        // ---- وقت الإنشاء ----
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ---- لو Structured Form ----
        public ICollection<SchoolRequestItems> Items { get; set; }
    }

}
