using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.data.Enum
{
    public enum SupplierOfferStatus
    {
        PendingAI = 1,             // إتبعت ولسه مستني تحليل AI
        PendingSupplierReview = 2, // AI حلل ومستني المورد يراجع
        SupplierConfirmed = 3,     // المورد أكد العرض
        ApprovedBySchool = 4,      // المدرسة وافقت
        RejectedBySchool = 5,      // المدرسة رفضت
        Expired = 6                // العرض انتهت صلاحيته
    }
}